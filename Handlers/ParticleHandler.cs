using System.Collections.Generic;
using System.Xml.Linq;
using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Handlers
{
    public abstract class ParticleHandler : StardustElement
    {

        public abstract void Reset();

        /// <summary>
        /// Invoked when each emitter step begins.
        /// </summary>
        public abstract void StepBegin(Emitter emitter, IEnumerable<Particle> particles, float time);

        /// <summary>
        /// Invoked when each emitter step ends.
        /// </summary>
        public abstract void StepEnd(Emitter emitter, IEnumerable<Particle> particles, float time);

        /// <summary>
        /// Invoked for each particle added.
        /// Handle particle creation in this method.
        /// </summary>
        public abstract void ParticleAdded(Particle particle);
        
        /// <summary>
        /// Invoked for each particle removed.
        /// Handle particle removal in this method.
        /// </summary>
        public abstract void ParticleRemoved(Particle particle);
        
        #region XML

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("handlers");
        }

        #endregion
        
    }
}