using System.Xml.Linq;

namespace Stardust.Clocks
{
    /// <summary>
    /// A clock is used by an emitter to determine how frequently particles are created.
    /// </summary>
    public abstract class Clock : StardustElement
    {

        /// <summary>
        /// On each <code>Emitter.Step()</code> call, this method is called.
        /// </summary>
        /// <param name="time">The timespan the emitter emitter's step.</param>
        /// <returns>tells the emitter how many particles it should create.</returns>
        public abstract int GetTicks(float time);

        /// <summary>
        /// Resets the clock and randomizes all values.
        /// </summary>
        public abstract void Reset();

        #region XML
        
        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("clocks");
        }
        
        #endregion
    }
}