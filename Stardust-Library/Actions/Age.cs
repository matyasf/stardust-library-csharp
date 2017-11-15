using System.Xml.Linq;
using Stardust.Emitters;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Actions
{
    /// <summary>
    /// Causes a particle's life to change (usually decrease).
    /// </summary>
    public class Age : Action
    {
        
        /// <summary>
        /// The multiplier of aging, 1 by default.
        ///
        /// <para>
        /// For instance, a multiplier value of 2 causes a particle to age twice as fast as normal.
        /// </para>
        ///
        /// <para>
        /// Alternatively, you can assign a negative value to the multiplier.
        /// This causes a particle's age to "increase".
        /// You can then use this increasing value with <code>LifeTrigger</code> and other custom actions to create various effects.
        /// </para>
        /// </summary>
        public float Multiplier;

        public Age() : this(1) {}
        
        public Age(float multiplier)
        {
            Multiplier = multiplier;
        }
        
        public override void Update(Emitter2D emitter, Particle particle, float timeDelta, float currentTime)
        {
            particle.Life -= timeDelta * Multiplier;
            if (particle.Life < 0) particle.Life = 0;
        }

        #region XML

        public override string GetXmlTagName()
        {
            return "Age";
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