using Stardust.Geom;

namespace Stardust.Actions.Areas
{
    public class RectArea : Area
    {

        public float Width;
        public float Height;
        
        public RectArea() : this(0, 0, 150, 50) {}
        
        public RectArea(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        
        public override bool Contains(float xc, float yc)
        {
            if (_rotation != 0) {
                // rotate the point backwards instead, it has the same result
                var vec = Vec2D.GetFromPool(xc, yc);
                vec.Rotate(-_rotation);
                xc = vec.X;
                yc = vec.Y;
            }
            if ((xc < X) || (xc > (X + Width))) return false;
            else if ((yc < Y) || (yc > (Y + Height))) return false;
            return true;
        }
    }
}