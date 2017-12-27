using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Initializers
{
    public class Alpha : Initializer
    {

        private RandomBase _random;

        public Alpha() : this(null) {}

        public Alpha(RandomBase random)
        {
            Random = random;
        }

        public override void Initialize(Particle particle)
        {
            particle.Alpha = _random.Random();
        }

        public RandomBase Random
        {
            get => _random;
            set
            {
                if (value == null)
                {
                    value = new UniformRandom(1, 0);
                }
                _random = value;
            }
        }
    }
}
