using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Initializers
{
    
    /// <summary>
    /// Sets a particle's mass value based on the <code>random</code> property.
    ///
    /// <para>
    /// A particle's mass is important in collision and gravity simulation.
    /// </para>
    /// </summary>
    public class Mass : Initializer
    {
        
        private RandomBase _random;
        
        public Mass() : this(null) {}
        
        public Mass(RandomBase random)
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
            particle.Mass = _random.Random();
        }

        
    }
}