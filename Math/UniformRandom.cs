using System.Xml.Linq;
using Stardust.Xml;

namespace Stardust.Math
{
    /// <summary>
    /// This class generates uniformly distributed random numbers.
    /// </summary>
    public class UniformRandom : RandomBase
    {
        /// <summary>
        /// The expected value of the random number.
        /// </summary>
        public float Center;

        /// <summary>
        /// The variation of the random number.
        ///
        /// <para>
        /// The range of the generated random number is [center - radius, center + radius].
        /// </para>
        /// </summary>
        public float Radius;

        public UniformRandom() : this(0.5f, 0) {}
        
        public UniformRandom(float center, float radius)
        {
            Center = center;
            Radius = radius;
        }
        
        public override float Random()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (Radius != 0f)
            {
                return (float)(Radius * 2 * (RandomGen.NextDouble() - 0.5) + Center);
            }
            return Center;
        }
        
        public override void SetRange(float lowerBound, float upperBound)
        {
            float diameter = upperBound - lowerBound;
            Radius = 0.5f * diameter;
            Center = lowerBound + Radius;
        }

        public override float[] GetRange()
        {
            return new[]{Center - Radius, Center + Radius};
        }

        #region XML

        public override string GetXmlTagName()
        {
            return "UniformRandom";
        }

        public override XElement ToXml()
        {
            XElement xml = base.ToXml();
            xml.SetAttributeValue("center", Center);
            xml.SetAttributeValue("radius", Radius);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            Center = float.Parse(xml.Attribute("center").Value);
            Radius = float.Parse(xml.Attribute("radius").Value);
        }

        #endregion
        

    }
}