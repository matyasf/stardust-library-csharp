using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using Stardust.MathStuff;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Initializers
{
    /// <summary>
    /// Sets a particle's initial scale based on the <code>random</code> property.
    /// </summary>
    public class Scale : Initializer
    {
        private RandomBase _random;
        
        public Scale() : this(null) {}
        
        public Scale(RandomBase random)
        {
            Random = random;
        }
        
        public RandomBase Random
        {
            get => _random;
            set
            {
                if (value == null)
                {
                    value = new UniformRandom(1, 0);
                }
                _random = value;
            }
        }

        public override void Initialize(Particle particle)
        {
            particle.InitScale = particle.Scale = _random.Random();
        }

        #region XML

        public override IEnumerable<StardustElement> GetRelatedObjects()
        {
            return new List<StardustElement>() { _random };
        }

        public override string GetXmlTagName()
        {
            return "Scale";
        }

        public override XElement ToXml()
        {
            var xml = base.ToXml();
            xml.SetAttributeValue("random", _random.Name);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);

            Random = (RandomBase)builder.GetElementByName(xml.Attribute("random").Value);
        }

        #endregion
    }
}