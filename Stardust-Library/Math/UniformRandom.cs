using System.Collections.Generic;
using System.Xml.Linq;
using Stardust.Xml;

namespace Stardust.Math
{
    public class UniformRandom : RandomBase
    {

        public UniformRandom(float a, float b)
        {
            
        }
        
        public override void ParseXml(XElement xml, XmlBuilder builder = null)
        {
            throw new System.NotImplementedException();
        }

        public override float Random()
        {
            throw new System.NotImplementedException();
        }

        public override void SetRange(float lowerBound, float upperBound)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<float> GetRange()
        {
            throw new System.NotImplementedException();
        }
    }
}