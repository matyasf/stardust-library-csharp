using System;
using System.Xml.Linq;
using NUnit.Framework;
using Stardust;
using Stardust.Xml;

namespace Stardust_Library_Tests.Xml
{
    [TestFixture]
    public class Tests
    {
        private readonly string _XmlGood = 
            @"<StardustParticleSystem version='2.1'>
                <actions> 
                    <StardustElement name='sde'/>
                </actions>
            </StardustParticleSystem>";
            
        [Test]
        public void RegisterClass_WrongType()
        {
            XmlBuilder builder = new XmlBuilder();
            Assert.Throws<InvalidOperationException>(() => builder.RegisterClass(typeof(PlainClass)));
        }
        
        [Test]
        public void RegisterClass_Duplicate()
        {
            XmlBuilder builder = new XmlBuilder();
            builder.RegisterClass(typeof(StardustClass));
            Assert.Throws<InvalidOperationException>(() => builder.RegisterClass(typeof(StardustClass)));
        }
        
        [Test]
        public void BuildXml()
        {
            XmlBuilder builder = new XmlBuilder();
            builder.RegisterClass(typeof(StardustClass));
            builder.BuildFromXml(XDocument.Parse(_XmlGood).Root);
        }
        
        [Test]
        public void BuildXml_NoClassRegistered()
        {
            XmlBuilder builder = new XmlBuilder();            
            Assert.Throws<NotSupportedException>(() => builder.BuildFromXml(XDocument.Parse(_XmlGood).Root));
        }
        
        [Test]
        public void BuildXml_NoName()
        {
            const string xmlNoName = @"<StardustParticleSystem version='2.1'>
                                        <actions> 
                                            <StardustElement/>
                                        </actions>
                                    </StardustParticleSystem>";
            XmlBuilder builder = new XmlBuilder();
            builder.RegisterClass(typeof(StardustClass));
            Assert.Throws<InvalidOperationException>(() => builder.BuildFromXml(XDocument.Parse(xmlNoName).Root));
        }
        
        [Test]
        public void BuildXml_DuplicateNames()
        {
            const string xmlDuplicateName = @"<StardustParticleSystem version='2.1'>
                                                <actions>
                                                    <StardustElement name='sde'/>
                                                    <StardustElement name='sde'/>
                                                </actions>
                                            </StardustParticleSystem>";
            XmlBuilder builder = new XmlBuilder();
            builder.RegisterClass(typeof(StardustClass));
            Assert.Throws<InvalidOperationException>(() => builder.BuildFromXml(XDocument.Parse(xmlDuplicateName).Root));
        }
    }

    internal class PlainClass{}

    internal class StardustClass : StardustElement{
        public override string GetXmlTagName()
        {
            return "StardustElement";
        }

        public override XElement GetElementTypeXmlTag()
        {
            return new XElement("no name");
        }

        public override void ParseXml(XElement xml, XmlBuilder builder) {}
    }
}