using Stardust.Geom;
using Stardust.Particles;

namespace Stardust.Actions.Deflectors
{
    /// <summary>
    /// Infinitely long line-shaped obstacle.
    /// One side of the line is free space, and the other side is "solid",
    /// not allowing any particle to go through.
    /// The line is defined by a point it passes through and its normal vector.
    ///
    /// <para>
    /// When a particle hits the border, it bounces back.
    /// </para>
    /// </summary>
    public class LineDeflector : Deflector
    {
        /// <summary>
        /// The X coordinate of a point the border passes through.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y coordinate of a point the border passes through.
        /// </summary>
        public float Y;

        private Vec2D _normal;
        
        public LineDeflector() : this(0, 0, 0, -1) {}
        
        public LineDeflector(float x, float y, float nx, float ny)
        {
            X = x;
            Y = y;
            _normal = Vec2D.GetFromPool(nx, ny);
        }

        public float NormalX
        {
            get => _normal.X;
            set => _normal.X = value;
        }
        
        public float NormalY
        {
            get => _normal.Y;
            set => _normal.Y = value;
        }
        
        protected override Vec4D CalculateMotionData4D(Particle particle)
        {
            // normal displacement
            var r = Vec2D.GetFromPool(particle.X - X, particle.Y - Y);
            r = r.Project(_normal);

            var dot = r.Dot(_normal);
            var radius = particle.CollisionRadius * particle.Scale;
            var dist = r.Length;

            if (dot > 0) 
            {
                if (dist > radius)
                {
                    // no collision detected
                    Vec2D.RecycleToPool(r);
                    return null;
                }
                r.Length = radius - dist;
            } else
            {
                // collision detected
                r.Length = -(dist + radius);
            }

            var v = Vec2D.GetFromPool(particle.Vx, particle.Vy);
            v = v.Project(_normal);

            var factor = 1 + Bounce;

            Vec2D.RecycleToPool(r);
            Vec2D.RecycleToPool(v);
            return Vec4D.GetFromPool(particle.X + r.X, particle.Y + r.Y,
                (particle.Vx - v.X * factor) * Slipperiness, 
                (particle.Vy - v.Y * factor) * Slipperiness);

        }
    }
}