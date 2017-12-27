using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions
{
    /// <summary>
    ///  Causes a particle's position to change according to its velocity.
    ///
    /// <para>
    /// Default priority = -4;
    /// </para>
    /// </summary>
    public class Move : Action
    {
        /// <summary>
        /// The multiplier of movement, 1 by default.
        ///
        /// <p>
        /// For instance, a multiplier value of 2 causes a particle to move twice as fast as normal.
        /// </p>
        /// </summary>
        public float Multiplier;
        
        private float _factor;
        
        public Move() : this(1) {}
        
        public Move(float multiplier)
        {
            _priority = -4;
            Multiplier = multiplier;
        }

        public override void PreUpdate(Emitter emitter, float time)
        {
            _factor = time * Multiplier;
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            particle.X += particle.Vx * _factor;
            particle.Y += particle.Vy * _factor;
        }

    }
}