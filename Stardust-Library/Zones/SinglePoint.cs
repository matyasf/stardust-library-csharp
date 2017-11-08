using System.Xml.Linq;
using Stardust.Geom;
using Stardust.Math;
using Stardust.Xml;

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
        
        public override bool Contains(float x, float y)
        {
            if (_x == x && _y == y) return true;
            return false;
        }
        
        public override Vec2D CalculateMotionData2D()
        {
            return Vec2D.Pool.Acquire();
        }
        
        protected override void UpdateArea()
        {
            area = VirtualThickness * VirtualThickness * StardustMath.Pi;
        }

        #region XML

        public override string GetXmlTagName()
        {
            return "SinglePoint";
        }

        public override XElement ToXml()
        {
            var xml = base.ToXml();
            xml.SetAttributeValue("x", _x);
            xml.SetAttributeValue("y", _y);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);
            _x = float.Parse(xml.Attribute("x").Value);
            _y = float.Parse(xml.Attribute("y").Value);
        }

        #endregion
        
        
    }
}