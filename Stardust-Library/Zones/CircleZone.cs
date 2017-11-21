
using System;
using Stardust.Geom;
using Stardust.MathStuff;

namespace Stardust.Zones
{
    /// <summary>
    /// Circular zone.
    /// </summary>
    class CircleZone : Zone
    {
        private float _radius;
        private float _radiusSQ;
        private static Random rng = new Random();
        
        public CircleZone() : this(0, 0, 100) {}
        
        public CircleZone(float x, float y, float radius)
        {
            _x = x;
            _y = y;
            Radius = radius;
        }
        
        /// <summary>
        /// The radius of the zone.
        /// </summary>
        public float Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                _radiusSQ = value * value;
                UpdateArea();
            }
        }

        public override Vec2D CalculateMotionData2D()
        {
            float theta = (float)(StardustMath.TwoPi * rng.NextDouble());
            float r = _radius * (float)Math.Sqrt(rng.NextDouble());
            return Vec2D.GetFromPool(r * (float)Math.Cos(theta), r * (float)Math.Sin(theta));
        }

        protected override void UpdateArea()
        {
            area = _radiusSQ * StardustMath.Pi;
        }
    }
}
