using System.Collections.Generic;
using System.Xml.Linq;

namespace Stardust.Math
{
    /// <summary>
    /// This class generates a random number.
    /// </summary>
    public abstract class RandomBase : StardustElement
    {

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
        public abstract IEnumerable<float> GetRange(); // TODO might be better as primitive array?

        #region XML

        public override string GetXmlTagName()
        {
            return "Random";
        }

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("randoms");
        }

        #endregion
        
    }
}