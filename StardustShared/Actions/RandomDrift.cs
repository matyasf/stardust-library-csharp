using Stardust.Emitters;
using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Actions
{
    /// <summary>
    /// Applies random acceleration to particles.
    /// </summary>
    public class RandomDrift : Action
    {
        /// <summary>
        /// Whether the particles acceleration is divided by their masses before applied to them, true by default.
        /// When set to true, it simulates a gravity that applies equal acceleration on all particles.
        /// </summary>
        public bool Massless;
        private RandomBase _randomX;
        private RandomBase _randomY;
        private float _timeDeltaOneSec;
        
        public RandomDrift() : this(null, null) {}
        
        public RandomDrift(RandomBase randomX, RandomBase randomY)
        {
            _priority = -3;
            Massless = true;
            RandomX = randomX;
            RandomY = randomY;
        }

        public RandomBase RandomX
        {
            get => _randomX;
            set
            {
                if (value == null)
                {
                    value = new UniformRandom(0, 10);
                }
                _randomX = value;
            }
        }

        public RandomBase RandomY
        {
            get => _randomY;
            set
            {
                if (value == null)
                {
                    value = new UniformRandom(0, 10);
                }
                _randomY = value;
            }
        }

        public override void PreUpdate(Emitter emitter, float time)
        {
            _timeDeltaOneSec = time * 60;
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            var rx = _randomX.Random();
            var ry = _randomY.Random();

            if (!Massless)
            {
                var factor = 1 / particle.Mass;
                rx *= factor;
                ry *= factor;
            }

            particle.Vx += rx * _timeDeltaOneSec;
            particle.Vy += ry * _timeDeltaOneSec;
        }
    }
}