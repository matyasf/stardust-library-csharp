using System;
using System.Xml.Linq;
using Stardust.Geom;
using Stardust.MathStuff;
using Stardust.Xml;
using Stardust.Zones;

namespace Stardust.Initializers
{
    
    /// <summary>
    /// Line segment zone.
    /// </summary>
    public class Line : Contour
    {
        public override float X
        {
            set
            {
                _x = value;
                UpdateArea();
            }
        }
        
        public override float Y
        {
            set
            {
                _y = value;
                UpdateArea();
            }
        }

        private float _x2;

        /// <summary>
        /// The X coordinate of the other end of the line.
        /// </summary>
        public float X2
        {
            get => _x2;
            set
            {
                _x2 = value;
                UpdateArea();
            }
        }
        
        private float _y2;

        /// <summary>
        /// The Y coordinate of the other end of the line.
        /// </summary>
        public float Y2
        {
            get => _y2;
            set
            {
                _y2 = value;
                UpdateArea();
            }
        }

        private RandomBase _random;
        
        public Line() : this(0, 0, 0, 0, null) {}
        
        public Line(float x1, float y1, float x2, float y2, RandomBase random)
        {
            _x = x1;
            _y = y1;
            _x2 = x2;
            _y2 = y2;
            Random = random;
        }

        public override void SetPosition(float xc, float yc)
        {
            var xDiff = _x2 - _x;
            var yDiff = _y2 - _y;
            _x = xc;
            _y = yc;
            _x2 = xc + xDiff;
            _y2 = yc + yDiff;
        }

        public RandomBase Random
        {
            get => _random;
            set
            {
                if (value == null)
                {
                    value = new UniformRandom();
                }
                _random = value;  
            } 
        }
        
        public override Vec2D CalculateMotionData2D()
        {
            _random.SetRange(0, 1);
            var rand = _random.Random();
            var vec = Vec2D.Pool.Acquire();
            vec.SetTo(StardustMath.Interpolate(0, 0, 1, _x2 - _x, rand), 
                      StardustMath.Interpolate(0, 0, 1, _y2 - _y, rand));
            return vec;

        }
        
        public override bool Contains(float x, float y)
        {
            if (x < _x && x < _x2) return false;
            if (x > _x && x > _x2) return false;
            if ((x - _x) / (_x2 - _x) == (y - _y) / (_y2 - _y)) return true;
            return false;
        }

        protected override void UpdateArea()
        {
            var dx = _x - _x2;
            var dy = _y - _y2;
            area = (float)Math.Sqrt(dx * dx + dy * dy) * VirtualThicknessVal;
        }

        #region XML

        public override string GetXmlTagName()
        {
            return "Line";
        }

        public override XElement ToXml()
        {
            var xml = base.ToXml();
            xml.SetAttributeValue("x1", _x);
            xml.SetAttributeValue("y1", _y);
            xml.SetAttributeValue("x2", _x2);
            xml.SetAttributeValue("y2", _y2);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);
            _x = float.Parse(xml.Attribute("x1").Value);
            _y = float.Parse(xml.Attribute("y1").Value);
            _x2 = float.Parse(xml.Attribute("x2").Value);
            _y2 = float.Parse(xml.Attribute("y2").Value);
        }

        #endregion
        
    }
}