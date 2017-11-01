using System;

namespace Stardust.Particles
{
    /// <summary>
    /// This class represents a particle and its properties.
    /// </summary>
    public class Particle : IComparable<Particle>
// TODO Make its constructor private?
    {

        /// <summary>
        /// The initial life upon birth.
        /// </summary>
        public float InitLife;

        /// <summary>
        /// The normal scale upon birth.
        /// </summary>
        public float InitScale;

        /// <summary>
        /// The alpha value upon birth. Deprecated!
        /// </summary>
        public float InitAlpha;

        /// <summary>
        /// The remaining life of the particle.
        /// </summary>
        public float Life;

        /// <summary>
        /// The scale of the particle.
        /// </summary>
        public float Scale;

        /// <summary>
        /// The alpha value of the particle.
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
        /// The collision radius of the particle.
        /// </summary>
        public float CollisionRadius;

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
        /// Initializes properties to default values.
        /// </summary>
        public void Init()
        {
            InitLife = Life = CurrentAnimationFrame = 0;
            InitScale = Scale = 1;
            InitAlpha = Alpha = 1;
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

        public void Destroy() // TODO is this nedeed?
        {
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