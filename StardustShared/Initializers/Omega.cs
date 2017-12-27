using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Initializers
{
    /// <summary>
    /// Sets a particle's initial omega value (rotation speed), in degrees per second,
    /// based on the <code>random</code> property.
    /// </summary>
    public class Omega : Initializer
    {

        private RandomBase _random;
        
        public Omega() : this(null) {}
        
        public Omega(RandomBase random)
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
                    value = new UniformRandom(0, 0);
                }
                _random = value;
            }
        }

        public override void Initialize(Particle particle)
        {
            particle.Omega = _random.Random();
        }
        
    }
}