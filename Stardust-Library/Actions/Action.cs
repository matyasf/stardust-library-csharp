using System;
using System.Xml.Linq;
using Stardust.Xml;

namespace Stardust.Actions
{
    /// <summary>
    /// An action is used to continuously update a particle's property.
    ///
    /// <para>
    /// An action is associated with an emitter. On each <code>Emitter.step()</code> method call,
    /// the action's <code>update()</code> method is called with each particles in the emitter passed in as parameter.
    /// This method updates a particles property, such as changing the particle's position according to its velocity,
    /// or modifying the particle's velocity based on gravity fields.
    /// </para>
    ///
    /// <para>Default priority = 0;</para>
    /// </summary>
    public class Action : StardustElement
    {
        
        /// <summary>
        /// Denotes if the action is active, true by default.
        /// </summary>
        public bool Active;

        protected int Priority;

        public Action()
        {
            Priority = 0;
            Active = true;
        }
        
        /**
         * [Template Method] This method is called once upon each <code>Emitter.step()</code> method call,
         * before the <code>update()</code> calls with each particles in the emitter.
         *
         * <p>
         * All setup operations before the <code>update()</code> calls should be done here.
         * </p>
         * @param    emitter        The associated emitter.
         * @param    time        The timespan of each emitter's step.
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="???"></param>
        /// <returns></returns>
        public function preUpdate(emitter : Emitter, time : Number) : void
        {
            //abstract method
        }
        
        public override void ParseXml(XElement xml, XmlBuilder builder = null)
        {
            Active = bool.Parse(xml.Attribute("name").Value);
        }
    }
}