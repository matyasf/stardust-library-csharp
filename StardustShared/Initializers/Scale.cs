using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Initializers
{
    /// <summary>
    /// Sets a particle's initial scale based on the <code>random</code> property.
    /// </summary>
    public class Scale : Initializer
    {
        private RandomBase _random;
        
        public Scale() : this(null) {}
        
        public Scale(RandomBase random)
        {
            Random = random;
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

        public override void Initialize(Particle particle)
        {
            particle.Scale = _random.Random();
        }
    }
}