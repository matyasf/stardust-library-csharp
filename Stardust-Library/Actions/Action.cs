
using System.Xml.Linq;
using Stardust.Emitters;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Actions
{
    /// <summary>
    /// An action is used to continuously update a particle's property.
    ///
    /// <para>
    /// An action is associated with an emitter. On each <code>Emitter.Step()</code> method call,
    /// the action's <code>Update()</code> method is called with each particles in the emitter passed in as parameter.
    /// This method updates a particles property, such as changing the particle's position according to its velocity,
    /// or modifying the particle's velocity based on gravity fields.
    /// </para>
    ///
    /// <para>Default priority = 0;</para>
    /// </summary>
    public abstract class Action : StardustElement
    {
       
        public delegate void ActionHandler(Action action);
        public static event ActionHandler Added;
        public static event ActionHandler Removed;
        public static event ActionHandler PriorityChange;
        
        /// <summary>
        /// Denotes if the action is active, true by default.
        /// </summary>
        public bool Active;

        protected int _priority;

        protected Action()
        {
            _priority = 0;
            Active = true;
        }
        
        /// <summary>
        /// [Template Method] This method is called once upon each <code>Emitter.Step()</code> method call,
        /// before the <code>update()</code> calls with each particles in the emitter.
        ///
        /// <para>
        /// All setup operations before the <code>Update()</code> calls should be done here.
        /// </para>
        /// </summary>
        /// <param name="emitter">The associated emitter.</param>
        /// <param name="time">The timespan of each emitter's step.</param>
        public virtual void PreUpdate(Emitter emitter, float time) {}

        /// <summary>
        /// [Template Method] Acts on all particles upon each <code>Emitter.step()</code> method call.
        ///
        /// <para>
        /// Override this method to create custom actions.
        /// </para>
        /// </summary>
        /// <param name="emitter">The associated emitter.</param>
        /// <param name="particle">The associated particle.</param>
        /// <param name="timeDelta">The timespan of each emitter's step.</param>
        /// <param name="currentTime">The total time from the first Emitter.Step() call.</param>
        public abstract void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime);
        
        /// <summary>
        /// [Template Method] This method is called once after each <code>Emitter.Step()</code> method call,
        /// after the <code>Update()</code> calls with each particles in the emitter.
        ///
        /// <para>
        /// All setup operations after the <code>Update()</code> calls should be done here.
        /// </para>
        /// </summary>
        /// <param name="emitter">The associated emitter.</param>
        /// <param name="time">The timespan of each emitter's step.</param>
        public virtual void PostUpdate(Emitter emitter, float time) {}

        public int Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                PriorityChange?.Invoke(this);
            }
        }

        /// <summary>
        /// Tells the emitter whether this action requires that particles must be sorted before the <code>update()</code> calls.
        ///
        /// <para>
        /// For instance, the <code>Collide</code> action needs all particles to be sorted in X positions.
        /// </para>
        /// </summary>
        public virtual bool NeedsSortedParticles
        {
            get { return false; }
        }
        
        #region XML

        public override string GetXmlTagName()
        {
            return GetType().Name;
        }

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("elements");
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