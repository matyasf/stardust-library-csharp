using System.Xml.Linq;
using Stardust.Emitters;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Actions.Triggers
{
    /// <summary>
    /// This action trigger return true if a particle is dead.
    /// </summary>
    public class DeathTrigger : Trigger
    {
        
        public override bool TestTrigger(Emitter emitter, Particle particle, float time)
        {
            return particle.IsDead;
        }
        
        public override string GetXmlTagName()
        {
            return "DeathTrigger";
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            
        }

    }
}