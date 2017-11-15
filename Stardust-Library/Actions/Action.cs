
using System.Runtime.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json;
using Stardust.Emitters;
using Stardust.Particles;

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
    /// </summary>
    public abstract class Action : SortableElement
    {
        
        /// <summary>
        /// This method is called once upon each <code>Emitter.Step()</code> method call,
        /// before the <code>Update()</code> calls with each particles in the emitter.
        ///
        /// <para>
        /// All setup operations before the <code>Update()</code> calls should be done here.
        /// </para>
        /// </summary>
        /// <param name="emitter">The associated emitter.</param>
        /// <param name="time">The timespan of each emitter's step.</param>
        public virtual void PreUpdate(Emitter2D emitter, float time) {}

        /// <summary>
        /// Acts on all particles upon each <code>Emitter.Step()</code> method call.
        ///
        /// <para>
        /// Override this method to create custom actions.
        /// </para>
        /// </summary>
        /// <param name="emitter">The associated emitter.</param>
        /// <param name="particle">The associated particle.</param>
        /// <param name="timeDelta">The timespan of each emitter's step.</param>
        /// <param name="currentTime">The total time from the first Emitter.Step() call.</param>
        public abstract void Update(Emitter2D emitter, Particle particle, float timeDelta, float currentTime);
        
        /// <summary>
        /// This method is called once after each <code>Emitter.Step()</code> method call,
        /// after the <code>Update()</code> calls with each particles in the emitter.
        ///
        /// <para>
        /// All setup operations after the <code>Update()</code> calls should be done here.
        /// </para>
        /// </summary>
        /// <param name="emitter">The associated emitter.</param>
        /// <param name="time">The timespan of each emitter's step.</param>
        public virtual void PostUpdate(Emitter2D emitter, float time) {}
        
        /// <summary>
        /// Tells the emitter whether this action requires that particles must be sorted before the <code>update()</code> calls.
        ///
        /// <para>
        /// For instance, the <code>Collide</code> action needs all particles to be sorted in X positions.
        /// </para>
        /// </summary>
        [JsonIgnore]
        public virtual bool NeedsSortedParticles
        {
            get { return false; }
        }
        
        #region XML

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("elements");
        }

        #endregion
    }
}