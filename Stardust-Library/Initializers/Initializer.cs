
using System.Collections.Generic;
using System.Xml.Linq;
using Stardust.Particles;

namespace Stardust.Initializers
{
    /// <summary>
    /// An initializer is used to alter just once (i.e. initialize) a particle's properties upon the particle's birth.
    ///
    /// <para>
    /// An initializer can be associated with an emitter or a particle factory.
    /// </para>
    /// </summary>
    public abstract class Initializer : SortableElement
    {

        public void DoInitialize(IEnumerable<Particle> particles, float currentTime)
        {
            if (Active)
            {
                foreach (Particle particle in particles)
                {
                    Initialize(particle);
                }
            }
        }

        /// <summary>
        /// This is the method that alters a particle's properties.
        ///
        /// <para>Override this property to create custom initializers.</para>
        /// </summary>
        public virtual void Initialize(Particle particle)
        {
        }

        #region XML

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("initializers");
        }
        
        #endregion

    }
}