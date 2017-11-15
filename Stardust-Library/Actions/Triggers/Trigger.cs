using System.Xml.Linq;
using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions.Triggers
{
    public abstract class Trigger : StardustElement
    {
        public abstract bool TestTrigger(Emitter2D emitter, Particle particle, float time);

        #region XML

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("triggers");
        }

        #endregion
        
    }
}