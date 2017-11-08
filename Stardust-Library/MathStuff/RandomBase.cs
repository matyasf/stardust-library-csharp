using System;
using System.Xml.Linq;

namespace Stardust.MathStuff
{
    /// <summary>
    /// This class generates a random number.
    /// </summary>
    public abstract class RandomBase : StardustElement
    {

        protected static readonly Random RandomGen = new Random();
        /// <summary>
        /// Generates a random number.
        /// </summary>
        public abstract float Random();

        /// <summary>
        /// Sets the random number's range.
        /// </summary>
        public abstract void SetRange(float lowerBound, float upperBound);

        /// <summary>
        /// Returns the random number's range.
        /// </summary>
        public abstract float[] GetRange();

        #region XML

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("randoms");
        }

        #endregion
        
    }
}