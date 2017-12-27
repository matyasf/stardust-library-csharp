using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions.Triggers
{
    /// <summary>
    /// This trigger will be triggered when a particle is alive.
    /// </summary>
    public class LifeTrigger : Trigger
    {
        /// <summary>
        /// For this trigger to work, a particle's life must also be within the lower and upper bounds when this property is set to true,
        /// or outside of the range if this property is set to false.
        /// </summary>
        public bool TriggerWithinBounds;
        
        private float _lowerBound;
        private float _upperBound;
        
        public LifeTrigger() : this(0, 99999, true) {}
        
        public LifeTrigger(float lowerBound, float upperBound, bool triggerWithinBounds)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            TriggerWithinBounds = triggerWithinBounds;
        }
        
        
        public float LowerBound
        {
            get => _lowerBound;
            set
            {
                if (value > _upperBound) _upperBound = value;
                _lowerBound = value;
            }
        }

        public float UpperBound
        {
            get => _upperBound;
            set 
            {
                if (value < _lowerBound) _lowerBound = value;
                _upperBound = value;
            }
        }


        public override bool TestTrigger(Emitter emitter, Particle particle, float time)
        {
            if (TriggerWithinBounds)
            {
                if ((particle.Life >= _lowerBound) && (particle.Life <= _upperBound)) 
                {
                    return true;
                }
            } else 
            {
                if ((particle.Life < _lowerBound) || (particle.Life > _upperBound)) 
                {
                    return true;
                }
            }
            return false;
        }
    }
}