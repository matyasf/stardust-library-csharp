using System;
using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions
{
    /// <summary>
    /// Limits the particle's maximum travel speed
    /// </summary>
    public class SpeedLimit : Action
    {
        public float Limit;
        private float _limitSq;
        
        public SpeedLimit() : this(100) {}
        
        public SpeedLimit(float limit)
        {
            Limit = limit;
        }

        public override void PreUpdate(Emitter emitter, float time)
        {
            _limitSq = Limit * Limit;
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            var speedSq = particle.Vx * particle.Vx + particle.Vy * particle.Vy;
            if (speedSq > _limitSq) 
            {
                float factor = Limit / (float)Math.Sqrt(speedSq);
                particle.Vx *= factor;
                particle.Vy *= factor;
            }
        }
    }
}