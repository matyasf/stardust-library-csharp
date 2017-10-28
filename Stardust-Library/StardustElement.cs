
using System.Collections.Generic;
using System.Xml.Linq;
using Stardust.Xml;

namespace Stardust
{
    /// <summary>
    /// All Stardust elements are subclasses of this class.
    /// </summary>
    public abstract class StardustElement
    {
        private static readonly Dictionary<string, int> ElementCounter = new Dictionary<string, int>();
        public string Name;

        public StardustElement()
        {
            string str = GetXmlTagName();
            if (!ElementCounter.ContainsKey(str)) {
                ElementCounter[str] = 0;
            }
            else {
                ElementCounter[str]++;
            }
            Name = str + "_" + ElementCounter[str];
        }
    
        //XML
        //------------------------------------------------------------------------------------------------
    
        /// <summary>
        /// Returns the related objects of the element.
        /// <para>
        /// This tells the <code>XMLBuilder</code> which elements are related,
        /// so the builder can include them in the XML representation.
        /// </para>
        /// </summary>
        public virtual List<StardustElement> GetRelatedObjects()
        {
            return new List<StardustElement>();
        }
    
        /// <summary>
        /// Returns the name of the root node of the element's XML representation.
        /// </summary>
        public virtual string GetXmlTagName()
        {
            return "StardustElement";
        }
    
        /// <summary>
        /// Returns the root tag of the XML representation.
        /// </summary>
        public XElement GetXmlTag()
        {
            XElement xml = XElement.Parse("<" + GetXmlTagName() + "/>");
            xml.SetAttributeValue("name", Name);
            return xml;
        }
    
        /// <summary>
        /// Returns the tag for containing elements of the same type.
        /// </summary>
        public virtual XElement GetElementTypeXmlTag()
        {
            return new XElement("elements");
        }
    
        /// <summary>
        /// Generates XML representation.
        /// </summary>
        public virtual XElement ToXml()
        {
            return GetXmlTag();
        }
    
        /// <summary>
        /// Reconstructs the element from XML representations.
        /// </summary>
        public abstract void ParseXml(XElement xml, XmlBuilder builder = null);
    
        /// <summary>
        /// This is called when the whole simulation's XML parsing is complete
        /// </summary>
        public virtual void OnXmlInitComplete() {}
    
        //------------------------------------------------------------------------------------------------
        //end of XML
    }
}