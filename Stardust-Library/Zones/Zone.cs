using System.Xml.Linq;
using Stardust.Geom;
using Stardust.MathStuff;
using Stardust.Xml;

namespace Stardust.Zones
{
    public abstract class Zone : StardustElement
    {
        protected float _rotation;
        protected float angleCos;
        protected float angleSin;
        protected float area;

        protected float _x;

        public virtual float X
        {
            get => _x;
            set => _x = value;
        }
        
        protected float _y;

        public virtual float Y
        {
            get => _y;
            set => _y = value;
        }

        public Zone()
        {
            Rotation = 0;
        }

        /// <summary>
        /// Updates the area of the zone.
        /// </summary>
        protected abstract void UpdateArea();

        /// <summary>
        /// Determines if a point is contained in the zone, true if contained.
        /// </summary>
        public abstract bool Contains(float x, float y);

        public Vec2D GetPoint()
        {
            Vec2D md2D = CalculateMotionData2D();
            if (_rotation != 0) {
                float originalX = md2D.X;
                md2D.X = originalX * angleCos - md2D.Y * angleSin;
                md2D.Y = originalX * angleSin + md2D.Y * angleCos;
            }
            md2D.X = _x + md2D.X;
            md2D.Y = _y + md2D.Y;
            return md2D;
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                float valInRad = value * StardustMath.DegreeToRadian;
                angleCos = (float)System.Math.Cos(valInRad);
                angleSin = (float)System.Math.Sin(valInRad);
                _rotation = value;
            }
        }

        /// <summary>
        /// Returns a <code>Vec2D</code> object representing a random point in the zone
        /// without rotation and translation
        /// </summary>
        public abstract Vec2D CalculateMotionData2D();
        
        /// <summary>
        /// Returns the area of the zone.
        /// Areas are used by the <code>CompositeZone</code> class to determine which area is bigger and deserves more weight.
        /// </summary>
        public float GetArea()
        {
            return area;
        }

        /// <summary>
        /// Sets the position of this zone.
        /// </summary>
        public virtual void SetPosition(float xc, float yc)
        {
            X = xc;
            Y = yc;
        }

        /// <summary>
        /// Gets the position of this Zone
        /// </summary>
        public Vec2D GetPosition()
        {
            return Vec2D.GetFromPool(X, Y);
        }

        #region XML

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("zones");
        }

        public override XElement ToXml()
        {
            XElement xml = base.ToXml();
            xml.SetAttributeValue("rotation", _rotation);
            return xml;
        }
        
        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            Rotation = float.Parse(xml.Attribute("rotation").Value);
        }

        #endregion
    }
}