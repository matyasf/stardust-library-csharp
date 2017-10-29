using System.Collections.Generic;
using System.Xml.Linq;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Initializers
{
    /// <summary>
    /// An initializer is used to alter just once (i.e. initialize) a particle's properties upon the particle's birth.
    ///
    /// <para>
    /// An initializer can be associated with an emitter or a particle factory.
    /// </para>
    ///
    /// <para>
    /// Default priority = 0;
    /// </para>
    /// </summary>
    public class Initializer : StardustElement
    {
        
        public delegate void InitializerHandler(Initializer initializer);
        public static event InitializerHandler Added;
        public static event InitializerHandler Removed;
        public static event InitializerHandler PriorityChange;
        
        /// <summary>
        /// Denotes if the initializer is active, true by default.
        /// </summary>
        public bool Active;

        private int _priority;

        public Initializer()
        {
            _priority = 0;
            Active = true;
        }

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
        /// [Template Method] This is the method that alters a particle's properties.
        ///
        /// <para>Override this property to create custom initializers.</para>
        /// </summary>
        public virtual void Initialize(Particle particle)
        {
            
        }

        /// <summary>
        /// Initializers will be sorted according to their priorities.
        ///
        /// <para>
        /// This is important,
        /// since some initializers may rely on other initializers to perform initialization beforehand.
        /// You can alter the priority of an initializer, but it is recommended that you use the default values.
        /// </para>
        /// </summary>
        public int Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                PriorityChange?.Invoke(this);
            }
        }

        #region XML

        public override string GetXmlTagName()
        {
            return "Initializer";
        }

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("initializers");
        }

        public override XElement ToXml()
        {
            XElement xml = base.ToXml();
            xml.SetAttributeValue("active", Active);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder = null)
        {
            Active = bool.Parse(xml.Attribute("active").Value);
        }
        #endregion

    }
}