using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Stardust.Xml
{
    /// <summary>
    /// <code>XMLBuilder</code> can generate Stardust elements' XML representations and reconstruct 
    /// elements from existing XML data.
    ///
    /// <para>
    /// Every <code>StardustElement</code> objects can generate its XML representation through the 
    /// <code>StardustElement.toXML()</code> method.
    /// And they can reconstruct configurations from existing XML data through the 
    /// <code>StardustElement.parseXML()</code> method.
    /// </para>
    /// </summary>
    public class XMLBuilder
    {
        //XML building
        //------------------------------------------------------------------------------------------------

        /// <summary>
        /// Generate the XML representation of an Stardust element.
        ///
        /// <para>
        /// All related elements' would be included in the XML representation.
        /// </para>
        /// </summary>
        public static XElement BuildXml(StardustElement rootElement)
        {
            XElement root = new XElement("StardustParticleSystem");
            root.SetAttributeValue("version", StardustInfo.Version);
            

            Dictionary<string, StardustElement> relatedElements = new Dictionary<string, StardustElement>();
            TraverseRelatedObjects(rootElement, relatedElements);

            List<StardustElement> relatedElementsArray = new List<StardustElement>();
            StardustElement element;
            foreach (var elem in relatedElements) {
                relatedElementsArray.Add(elem.Value);
            }
            relatedElementsArray.Sort(ElementTypeSorter);

            foreach (var elem in relatedElementsArray) {
                var elementXml = elem.ToXml();
                var typeXml = elem.GetElementTypeXmlTag();
                if (!root.Elements(typeXml.Name).Any())
                {
                    root.Add(typeXml);
                }
                root.Element(typeXml.Name).Add(elementXml);
            }

            return root;
        }

        private static int ElementTypeSorter(StardustElement e1, StardustElement e2)
        {
            var comp = String.CompareOrdinal(e1.GetXmlTagName(), e2.GetXmlTagName());
            if (comp != 0) return comp;
            var comp2 = String.CompareOrdinal(e1.Name, e2.Name);
            if (comp2 == 1) return comp2;
            return -1;
        }
        
        private static void TraverseRelatedObjects(StardustElement element, Dictionary<string, StardustElement> relatedElements)
        {
            if (element == null) return;

            if (relatedElements.ContainsKey(element.Name)) {
                if (relatedElements[element.Name] != element) {
                    throw new Exception("Duplicate element name: " + element.Name + " " + relatedElements[element.Name]);
                }
            } else {
                relatedElements[element.Name] = element;
            }
            foreach (StardustElement e in element.GetRelatedObjects()) {
                TraverseRelatedObjects(e, relatedElements);
            }
        }
        
        //------------------------------------------------------------------------------------------------
        //end of XML building


        private Dictionary<string, Type> elementClasses;
        private Dictionary<string, StardustElement> elements;

        public XMLBuilder()
        {
            elementClasses = new Dictionary<string, Type>();
            elements = new Dictionary<string, StardustElement>();
        }
        
        /// <summary>
        /// To use <code>XMLBuilder</code> with your custom subclasses of Stardust elements,
        /// you must register your class and XML tag name first.
        ///
        /// <para>
        /// For example, if you register the <code>MyAction</code> class with XML tag name "HelloWorld",
        /// <code>XMLBuilder</code> knows you are referring to the <code>MyAction</code> class when a &ltHelloWorld&gt tag
        /// appears in the XML representation.
        /// </para>
        /// </summary>
        /// <exception cref="InvalidOperationException">If elementClass is not a subclass of StardustElement or its
        ///                                             already registered</exception>
        public void RegisterClass(Type elementClass)
        {
            var newInstance = Activator.CreateInstance(elementClass);
            if (!(newInstance is StardustElement)) 
            { 
                throw new InvalidOperationException(elementClass + " is not a subclass of the StardustElement class.");
            }
            StardustElement element = (StardustElement) newInstance;
            string tagName = element.GetXmlTagName();
            if (elementClasses.ContainsKey(tagName)) 
            {
                throw new InvalidOperationException("This element class name is already registered: " + element.GetXmlTagName());
            }
            elementClasses[tagName] = elementClass;
        }

        /// <summary>
        /// Register multiple classes
        /// </summary>
        public void RegisterClasses(IEnumerable<Type> classes)
        {
            foreach (Type type in classes)
            {
                RegisterClass(type);
            }
        }
        
        /// <summary>
        /// Registers multiple classes from a <code>ClassPackage</code> object.
        /// </summary>
        public void RegisterClassesFromClassPackage(ClassPackage classPackage)
        {
            RegisterClasses(classPackage.GetClasses());
        }

        /// <summary>
        /// Deletes the XML tag name registration
        /// </summary>
        public void UnregisterClass(string name)
        {
            elementClasses.Remove(name);
        }

        /// <summary>
        /// After reconstructing elements through the <code>BuildFromXML()</code> method,
        /// reconstructed elements can be extracted through this method.
        /// </summary>
        public StardustElement GetElementByName(string name)
        {
            return elements[name];
        }

        public IEnumerable<StardustElement> GetElementByClass(Type t)
        {
            var ret = new List<StardustElement>();
            foreach (var element in elements)
            {
                if (element.Value.GetType().Equals(t))
                {
                    ret.Add(element.Value);
                }    
            }
            return ret;
        }

        /// <summary>
        /// Reconstructs elements from XML representations.
        ///
        /// <para>
        /// After calling this method, you may extract constructed elements through the 
        /// <code>GetElementByName()</code> or the GetElementByClass() methods.</para>
        /// </summary>
        /// <exception cref="NotSupportedException">When an XML tag cannot be resolved</exception>
        /// <exception cref="InvalidOperationException">When multiple tags have the same name</exception>
        public void BuildFromXml(XElement xml)
        {
            elements = new Dictionary<string, StardustElement>();
            
            StardustElement element;
            // Collect
            foreach (XElement descendant in xml.Descendants())
            {
                foreach (XElement node in descendant.Descendants())
                {
                    try {
                        Type nodeClass = elementClasses[node.Name.LocalName];
                        element = (StardustElement)Activator.CreateInstance(nodeClass);
                    }
                    catch (Exception exception) {
                        throw new NotSupportedException("Unable to instantiate class " + node.Name.LocalName +
                                                        ". Perhaps you forgot to call XMLBuilder.RegisterClass() for this " +
                                                        "type? Original error: " + exception.ToString());
                    }
                    if (node.Attribute("name") == null)
                    {
                        throw new InvalidOperationException("Every grandchild of the root node must have a 'name' attribute " +
                                                            "Node" + node.Name.LocalName + " does not have one.");
                    }
                    if (elements.ContainsKey(node.Attribute("name").Value))
                    {
                        throw new InvalidOperationException("Duplicate XML node name for node '" + node.Name.LocalName + 
                                                            "' :" + node.Attribute("name").Value + " Node names must be unique");
                    }
                    elements[node.Attribute("name").Value] = element;
                }
            }
            foreach (XElement descendant in xml.Descendants())
            {
                foreach (XElement node in descendant.Descendants())
                {
                    element = elements[node.Attribute("name").Value];
                    element.ParseXml(node, this);   
                }
            }
            foreach (var stardustElement in elements) {
                stardustElement.Value.OnXmlInitComplete();
            }
        }
    }
}