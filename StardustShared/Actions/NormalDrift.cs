﻿using Stardust.Emitters;
using Stardust.Geom;
using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Actions
{
    /// <summary>
    /// Applies acceleration normal to a particle's velocity to the particle.
    /// </summary>
    public class NormalDrift : Action
    {
        
        /// <summary>
        /// Whether the particles acceleration is divided by their masses before applied to them, true by default.
        /// When set to true, it simulates a gravity that applies equal acceleration on all particles.
        /// </summary>
        public bool Massless;
        
        private float _timeDeltaOneSec;
        
        private RandomBase _random;
        
        public NormalDrift() : this(1, null) {}
        
        public NormalDrift(float max, RandomBase random)
        {
            Massless = true;
            Random = random;
        }
        
        /// <summary>
        /// The random object used to generate a random number for the acceleration, uniform random by default.
        /// </summary>
        public RandomBase Random
        {
            get => _random;
            set
            {
                if (value == null)
                {
                    value = new UniformRandom();
                }
                _random = value;
            }
        }

        public override void PreUpdate(Emitter emitter, float time)
        {
            _timeDeltaOneSec = time * 60;
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            var v = Vec2D.GetFromPool(particle.Vy, particle.Vx);
            v.Length = _random.Random();
            if (!Massless) v.Length /= particle.Mass;
            particle.Vx += v.X * _timeDeltaOneSec;
            particle.Vy += v.Y * _timeDeltaOneSec;
            Vec2D.RecycleToPool(v);
        }

    }
}