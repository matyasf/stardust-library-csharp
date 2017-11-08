using System.Xml.Linq;
using Stardust.Geom;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Fields
{
    /// <summary>
    /// 2D vector field
    /// </summary>
    public abstract class Field : StardustElement
    {
        public bool Active;
        public bool Massless;
        private Vec2D md2D;
        private float _invMass;

        public Field()
        {
            Active = true;
            Massless = true;
        }

        public Vec2D GetMotionData2D(Particle particle)
        {
            if (!Active) return Vec2D.GetFromPool();

            md2D = CalculateMotionData2D(particle);
            if (!Massless) {
                _invMass = 1 / particle.Mass;
                md2D.X *= _invMass;
                md2D.Y *= _invMass;
            }
            return md2D;
        }

        protected abstract Vec2D CalculateMotionData2D(Particle particle);

        /// <summary>
        /// Sets the position of this Field.
        /// </summary>
        public abstract void SetPosition(float xc, float yc);

        /// <summary>
        /// Gets the position of this Field.
        /// </summary>
        public abstract Vec2D GetPosition();

        #region XML

        public override string GetXmlTagName()
        {
            return "Field";
        }

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("fields");
        }

        public override XElement ToXml()
        {
            XElement xml = base.ToXml();
            xml.SetAttributeValue("active", Active);
            xml.SetAttributeValue("massless", Massless);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            Active = bool.Parse(xml.Attribute("active").Value);
            Massless = bool.Parse(xml.Attribute("massless").Value);
        }

        #endregion
    }
}