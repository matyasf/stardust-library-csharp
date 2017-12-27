using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions
{
    /// <summary>
    /// Causes a particle's rotation to change according to it's omega value (angular velocity).
    /// <para>
    /// Default priority = -4;
    /// </para>
    /// </summary>
    public class Spin : Action
    {
        
        /// <summary>
        /// The multiplier of spinning, 1 by default.
        /// <para>
        /// For instance, a multiplier value of 2 causes a particle to spin twice as fast as normal.
        /// </para>
        /// </summary>
        public float Multiplier;

        private float _factor;
        
        public Spin() : this(1) { }

        public Spin(float multiplier)
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
            particle.Rotation += particle.Omega * _factor;
        }
        
    }
}