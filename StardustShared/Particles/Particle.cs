using System;

namespace Stardust.Particles
{
    /// <summary>
    /// This class represents a particle and its properties.
    /// </summary>
    public class Particle : IComparable<Particle>
    {

        /// <summary>
        /// The initial life upon birth.
        /// </summary>
        public float InitLife;

        /// <summary>
        /// The remaining life of the particle.
        /// </summary>
        public float Life;

        /// <summary>
        /// The scale of the particle.
        /// </summary>
        public float Scale;

        /// <summary>
        /// The alpha value of the particle. (0..1)
        /// </summary>
        public float Alpha;
        
        /// <summary>
        /// The mass of the particle.
        /// </summary>
        public float Mass;
        
        /// <summary>
        /// Whether the particle is marked as dead.
        /// <para>
        /// Dead particles would be removed from simulation by an emitter.
        /// </para>
        /// </summary>
        public bool IsDead;

        /// <summary>
        /// The collision radius of the particle. Not set currently, but you can set it manually.
        /// </summary>
        public float CollisionRadius;
        
        /// <summary>
        /// Custom user data of the particle.
        /// <para>
        /// Normally, this property contains information for renderers.
        /// For instance this property should refer to the graphics object of this particle.
        /// </para>
        /// </summary>
        public object Target;

        /// <summary>
        /// Current red color component; in the [0,1] range.
        /// </summary>
        public float ColorR;
        
        /// <summary>
        /// Current green color component; in the [0,1] range.
        /// </summary>
        public float ColorG;
        
        /// <summary>
        /// Current blue color component; in the [0,1] range.
        /// </summary>
        public float ColorB;

        /// <summary>
        /// Particle handlers use this property to determine which frame to display if the particle is animated.
        /// </summary>
        public int CurrentAnimationFrame;

        public float X;
        public float Y;
        public float Vx;
        public float Vy;
        public float Rotation;
        public float Omega;
        
        /// <summary>
        /// Used by Spwn to determine if this particle is being deflected
        /// </summary>
        public bool IsDeflected = false;
        
        // Use PooledParticleFactory to instantiate Particles!
        internal Particle()
        {
        }
        
        /// <summary>
        /// Initializes properties to default values.
        /// </summary>
        public void Init()
        {
            InitLife = Life = CurrentAnimationFrame = 0;
            Scale = 1;
            Alpha = 1;
            Mass = 1;
            IsDead = false;
            CollisionRadius = 0;

            ColorR = 1;
            ColorB = 1;
            ColorG = 1;

            X = 0;
            Y = 0;
            Vx = 0;
            Vy = 0;
            Rotation = 0;
            Omega = 0;
        }

        public void Destroy() // is this nedeed?
        {
            Target = null;
        }

        public int CompareTo(Particle other)
        {
            if (X < other.X)
            {
                return -1;
            }
            return 1;
        }
    }
}