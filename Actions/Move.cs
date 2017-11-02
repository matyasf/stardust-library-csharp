using System.Xml.Linq;
using Stardust.Emitters;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Actions
{
    /// <summary>
    ///  Causes a particle's position to change according to its velocity.
    ///
    /// <para>
    /// Default priority = -4;
    /// </para>
    /// </summary>
    public class Move : Action
    {
        /// <summary>
        /// The multiplier of movement, 1 by default.
        ///
        /// <p>
        /// For instance, a multiplier value of 2 causes a particle to move twice as fast as normal.
        /// </p>
        /// </summary>
        public float Multiplier;

        private float factor;
        
        public Move() : this(1) {}
        
        public Move(float multiplier)
        {
            _priority = -4;
            Multiplier = multiplier;
        }

        public override void PreUpdate(Emitter emitter, float time)
        {
            factor = time * Multiplier;
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            particle.X += particle.Vx * factor;
            particle.Y += particle.Vy * factor;
        }

        #region XML

        public override string GetXmlTagName()
        {
            return "Move";
        }

        public override XElement ToXml()
        {
            XElement xml = base.ToXml();
            xml.SetAttributeValue("multiplier", Multiplier);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);
            Multiplier = float.Parse(xml.Attribute("multiplier").Value);
        }

        #endregion
    }
}