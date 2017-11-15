using System.Xml.Linq;
using System.Xml.Serialization;
using Stardust.Emitters;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Actions
{
    /// <summary>
    /// Causes a particle's rotation to change according to it's omega value (angular velocity).
    /// <para>
    /// Default priority = -4;
    /// </para>
    /// </summary>
    public class Spin : Action
    {
        
        /// <summary>
        /// The multiplier of spinning, 1 by default.
        /// <para>
        /// For instance, a multiplier value of 2 causes a particle to spin twice as fast as normal.
        /// </para>
        /// </summary>
        public float Multiplier;

        private float _factor;
        
        public Spin() : this(1) { }

        public Spin(float multiplier)
        {
            _priority = -4;
            Multiplier = multiplier;
        }

        public override void PreUpdate(Emitter2D emitter, float time)
        {
            _factor = time * Multiplier;
        }

        public override void Update(Emitter2D emitter, Particle particle, float timeDelta, float currentTime)
        {
            particle.Rotation += particle.Omega * _factor;
        }
        
        #region XML

        public override string GetXmlTagName()
        {
            return "Spin";
        }

        public override XElement ToXml()
        {
            var xml = base.ToXml();
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