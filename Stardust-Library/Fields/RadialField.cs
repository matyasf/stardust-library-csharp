using System;
using Stardust.Geom;
using Stardust.Particles;

namespace Stardust.Fields
{
    public class RadialField : Field
    {
        
        /// <summary>
        /// The X coordinate of the center of the field.
        /// </summary>
        public float X; 

        /// <summary>
        /// The Y coordinate of the center of the field.
        /// </summary>
        public float Y;

        /// <summary>
        /// The attenuation power of the field, in power per pixel.
        /// </summary>
        public float AttenuationPower;
        
        /// <summary>
        /// The strength of the field.
        /// </summary>
        public float Strength;

        /// <summary>
        /// If a point is closer to the center than this value,
        /// it's treated as if it's this far from the center.
        /// This is to prevent simulation from blowing up for points too near to the center.
        /// </summary>
        public float Epsilon;

        public RadialField(float x, float y, float strength, float attenuationPower, float epsilon)
        {
            X = x;
            Y = y;
            Strength = strength;
            AttenuationPower = attenuationPower;
            Epsilon = epsilon;
        }
        
        protected override Vec2D CalculateMotionData2D(Particle particle)
        {
            Vec2D r = Vec2D.GetFromPool(particle.X - X, particle.Y - Y);
            var len = r.Length;
            if (len < Epsilon) len = Epsilon;
            r.Length = Strength * (float)Math.Pow(len, -0.5 * AttenuationPower);
            return r;
        }

        public override void SetPosition(float xc, float yc)
        {
            X = xc;
            Y = yc;
        }

        public override Vec2D GetPosition()
        {
            return Vec2D.GetFromPool(X, Y);
        }
    }
}