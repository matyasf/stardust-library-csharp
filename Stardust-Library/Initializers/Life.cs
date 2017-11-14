using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using Stardust.MathStuff;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Initializers
{
    /// <summary>
    /// Sets a particle's life value based on the <code>random</code> property.
    /// </summary>
    public class Life : Initializer
    {
        private RandomBase _random;

        public Life() : this(null) {}
        
        public Life(RandomBase random)
        {
            Random = random;
        }

        public override void Initialize(Particle particle)
        {
            particle.InitLife = particle.Life = _random.Random();
        }
        
        /// <summary>
        /// A partilce's life is set according to this property.
        /// </summary>
        public RandomBase Random
        {
            get => _random;
            set
            {
                if (value == null)
                {
                    value = new UniformRandom(0, 0);
                }
                _random = value;
            }
        }

        #region XML

        public override IEnumerable<StardustElement> GetRelatedObjects()
        {
            var li = new List<StardustElement> {_random};
            return li;
        }

        public override string GetXmlTagName()
        {
            return "Life";
        }

        public override XElement ToXml()
        {
            XElement xml = base.ToXml();
            xml.SetAttributeValue("random", _random.Name);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);
            
            Random = builder.GetElementByName(xml.Attribute("random").Value) as RandomBase;
        }

        #endregion
    }
}