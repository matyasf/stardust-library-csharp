using Stardust.Geom;
using Stardust.MathStuff;

namespace Stardust.Zones
{
    public class SinglePoint : Contour
    {
        
        public SinglePoint() : this(0,0) {}
        
        public SinglePoint(float x, float y)
        {
            _x = x;
            _y = y;
            UpdateArea();
        }
        
        public override Vec2D CalculateMotionData2D()
        {
            return Vec2D.GetFromPool();
        }
        
        protected override void UpdateArea()
        {
            area = VirtualThickness * VirtualThickness * StardustMath.Pi;
        }

    }
}