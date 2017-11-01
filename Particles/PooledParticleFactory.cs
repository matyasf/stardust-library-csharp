using System.Collections.Generic;
using Stardust.Collections;
using Stardust.Initializers;

namespace Stardust.Particles
{
    public class PooledParticleFactory
    {
        public static readonly Pool<Particle> Pool = new Pool<Particle>(100, pool => new Particle());
        
        private readonly SortableCollection _initializerCollection;
        
        
        public PooledParticleFactory()
        {
            _initializerCollection = new SortableCollection();
        }
        
        /// <summary>
        /// Creates particles with associated initializers.
        /// </summary>
        /// <param name="count">The number to create.</param>
        /// <param name="currentTime">The current simulation time.</param>
        /// <param name="toList">The list the particles will be added to to prevent object allocation</param>
        /// <returns>The newly created particles.</returns>
        public List<Particle> CreatParticles(int count, float currentTime, List<Particle> toList)
        {
            List<Particle> particles = toList;
            if (particles == null)
            {
                particles = new List<Particle>();
            }
            if (count > 0) {
                for (int i = 0; i < count; i++)
                {
                    var particle = Pool.Acquire();
                    particle.Init();
                    particles.Add(particle);
                }

                var initializers = _initializerCollection.Elems;
                int len = initializers.Count;
                for (int i = 0; i < len; ++i)
                {
                    Initializer init = (Initializer) (initializers[i]);
                    init.DoInitialize(particles, currentTime);
                }
            }
            return particles;
        }
        
        /// <summary>
        /// Adds an initializer to the factory.
        /// </summary>
        public void AddInitializer(Initializer initializer)
        {
            _initializerCollection.Add(initializer);
        }

        public void RemoveInitializer(Initializer initializer)
        {
            _initializerCollection.Remove(initializer);
        }

        public void ClearInitializers()
        {
            _initializerCollection.Clear();
        }
        
        public SortableCollection InitializerCollection => _initializerCollection;

        public void Recycle(Particle particle)
        {
            Pool.Release(particle);
        }
    }
}