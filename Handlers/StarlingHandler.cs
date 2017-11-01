using System.Collections.Generic;
using System.Xml.Linq;
using Stardust.Emitters;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Handlers
{
    public class StarlingHandler : ParticleHandler // TODO
    {

        public override void Reset()
        {
        }

        public override void StepBegin(Emitter emitter, IEnumerable<Particle> particles, float time)
        {
        }

        public override void StepEnd(Emitter emitter, IEnumerable<Particle> particles, float time)
        {
        }

        public override void ParticleAdded(Particle particle)
        {
        }

        public override void ParticleRemoved(Particle particle)
        {   
        }

        #region XML

        public override string GetXmlTagName()
        {
            return "StarlingHandler";
        }

        public override XElement ToXml()
        {
            return base.ToXml();
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
        }

        #endregion
    }
}