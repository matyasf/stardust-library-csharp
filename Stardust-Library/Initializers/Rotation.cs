﻿using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Initializers
{
    /// <summary>
    /// Sets a particle's initial rotation value, in degrees, based on the <code>random</code> property.
    /// </summary>
    public class Rotation : Initializer
    {
        private RandomBase _random;
        
        public Rotation() : this(null) {}
        
        public Rotation(RandomBase random)
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
            particle.Rotation = _random.Random();
        }

    }
}