using Stardust.Geom;
using Stardust.Particles;

namespace Stardust.Actions.Deflectors
{
    /// <summary>
    /// Circular obstacle.
    ///
    /// <para>
    /// When a particle hits the obstacle, it bounces back.
    /// </para>
    /// </summary>
    public class CircleDeflector : Deflector
    {
        public float X;

        public float Y;

        public float Radius;
        
        public CircleDeflector() : this(0, 0, 100) {}
        
        public CircleDeflector(float x, float y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }
        
        protected override Vec4D CalculateMotionData4D(Particle particle)
        {
            //normal displacement
            var cr = particle.CollisionRadius * particle.Scale;
            var r = Vec2D.GetFromPool(particle.X - X, particle.Y - Y);

            //no collision detected
            var len = r.Length - cr;
            if (len > Radius)
            {
                Vec2D.RecycleToPool(r);
                return null;
            }

            //collision detected
            r.Length = Radius + cr;
            var v = Vec2D.GetFromPool(particle.Vx, particle.Vy);
            v.ProjectThis(r);

            var factor = 1 + Bounce;

            Vec2D.RecycleToPool(r);
            Vec2D.RecycleToPool(v);
            return Vec4D.GetFromPool(X + r.X, Y + r.Y,
                (particle.Vx - v.X * factor) * Slipperiness, 
                (particle.Vy - v.Y * factor) * Slipperiness);
        }
    }
}