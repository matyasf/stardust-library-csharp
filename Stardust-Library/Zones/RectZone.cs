using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using Stardust.Geom;
using Stardust.MathStuff;
using Stardust.Xml;

namespace Stardust.Zones
{
    /// <summary>
    /// Rectangular zone
    /// </summary>
    public class RectZone : Zone
    {

        private RandomBase _randomX;
        private RandomBase _randomY;
        private float _width;
        private float _height;
        
        public RectZone() : this(0, 0, 150, 50, null, null) {}
        
        public RectZone(float x, float y, float width, float height, RandomBase randomX, RandomBase randomY)
        {
            if (randomX == null) randomX = new UniformRandom();
            if (randomY == null) randomY = new UniformRandom();

            _x = x;
            _y = y;
            RandomX = randomX;
            RandomY = randomY;
            Width = width;
            Height = height;
        }
        
        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                _randomX.SetRange(0, value);
                UpdateArea();
            }
        }
        
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                _randomY.SetRange(0, value);
                UpdateArea();
            }
        }
        
        public RandomBase RandomX
        {
            get => _randomX;
            set
            {
                if (value == null) value = new UniformRandom();
                _randomX = value;
                _randomX.SetRange(0, _width);
            }
        }
        
        public RandomBase RandomY
        {
            get => _randomY;
            set
            {
                if (value == null) value = new UniformRandom();
                _randomY = value;
                _randomY.SetRange(0, _height);
            }
        }
        
        protected override void UpdateArea()
        {
            area = _width * _height;
        }

        public override Vec2D CalculateMotionData2D()
        {
            return Vec2D.GetFromPool(_randomX.Random(), _randomY.Random());
        }
        
        public override bool Contains(float xc, float yc)
        {
            if (_rotation != 0) {
                // rotate the point backwards instead, it has the same result
                Vec2D vec = Vec2D.GetFromPool(xc, yc);
                vec.Rotate(-_rotation);
                xc = vec.X;
                yc = vec.Y;
            }
            if ((xc < _x) || (xc > (_x + _width))) return false;
            else if ((yc < _y) || (yc > (_y + _height))) return false;
            return true;
        }

        #region XML

        public override IEnumerable<StardustElement> GetRelatedObjects()
        {
            return new List<StardustElement>() {_randomX, _randomY};
        }
        
        public override string GetXmlTagName()
        {
            return "RectZone";
        }

        public override XElement ToXml()
        {
            var xml = base.ToXml();
            
            xml.SetAttributeValue("x", _x);
            xml.SetAttributeValue("y", _y);
            xml.SetAttributeValue("width", _width);
            xml.SetAttributeValue("height", _height);
            xml.SetAttributeValue("randomX", _randomX.Name);
            xml.SetAttributeValue("randomY", _randomY.Name);

            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);
            
            _x = float.Parse(xml.Attribute("x").Value);
            _y = float.Parse(xml.Attribute("y").Value);
            Width = float.Parse(xml.Attribute("width").Value);
            Height = float.Parse(xml.Attribute("height").Value);
            RandomX = (RandomBase)builder.GetElementByName(xml.Attribute("randomX").Value);
            RandomY = (RandomBase)builder.GetElementByName(xml.Attribute("randomY").Value);
        }

        #endregion
    }
}