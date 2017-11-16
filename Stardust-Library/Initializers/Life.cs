using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Initializers
{
    /// <summary>
    /// Sets a particle's life value based on the <code>random</code> property.
    /// </summary>
    public class Life : Initializer
    {
        private RandomBase _random;

        public Life() : this(null) {}
        
        public Life(RandomBase random)
        {
            Random = random;
        }

        public override void Initialize(Particle particle)
        {
            particle.InitLife = particle.Life = _random.Random();
        }
        
        /// <summary>
        /// A partilce's life is set according to this property.
        /// </summary>
        public RandomBase Random
        {
            get => _random;
            set
            {
                if (value == null)
                {
                    value = new UniformRandom(0, 0);
                }
                _random = value;
            }
        }
    }
}