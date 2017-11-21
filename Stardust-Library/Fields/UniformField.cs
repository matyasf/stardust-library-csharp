using Stardust.Geom;
using Stardust.Particles;

namespace Stardust.Fields
{
    /// <summary>
    /// Uniform vector field. It yields a <code>MotionData2D</code> object of same X and Y components no matter what.
    ///
    /// <para>
    /// This can be used to simulate uniform gravity.
    /// </para>
    /// </summary>
    public class UniformField : Field
    {
        
        /// <summary>
        /// The X component of the returned <code>Vec2D</code> object.
        /// </summary>
        public float X;

        /// <summary>
        /// The Y component of the returned <code>Vec2D</code> object.
        /// </summary>
        public float Y;
        
        public UniformField() : this(0, 0) {}
        
        public UniformField(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        protected override Vec2D CalculateMotionData2D(Particle particle)
        {
            return Vec2D.GetFromPool(X, Y);
        }

        public override void SetPosition(float xc, float yc)
        {
            // do nothing, position can not be set on this field.
        }

        public override Vec2D GetPosition()
        {
            return Vec2D.GetFromPool(0, 0);
        }
    }
}