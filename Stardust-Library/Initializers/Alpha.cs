using System.Collections.Generic;
using System.Xml.Linq;
using Stardust.Math;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Initializers
{
    public class Alpha : Initializer
    {

        private RandomBase _random;

        public Alpha() : this(null) {}

        public Alpha(RandomBase random)
        {
            Random = random;
        }

        public override void Initialize(Particle particle)
        {
            particle.InitAlpha = particle.Alpha = _random.Random();
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

        #region XML

        public override IEnumerable<StardustElement> GetRelatedObjects()
        {
            return new List<StardustElement>() { _random };
        }

        public override string GetXmlTagName()
        {
            return "Alpha";
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
