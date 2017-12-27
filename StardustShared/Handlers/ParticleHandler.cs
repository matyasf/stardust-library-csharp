using System.Collections.Generic;
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
        public virtual void StepBegin(Emitter emitter, IList<Particle> particles, float time) {}

        /// <summary>
        /// Invoked when each emitter step ends.
        /// </summary>
        public abstract void StepEnd(Emitter emitter, IList<Particle> particles, float time);

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
        
    }
}