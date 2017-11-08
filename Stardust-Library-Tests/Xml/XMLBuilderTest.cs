using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
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
        
        /*
        [Test]
        public void XmlSerializerTest()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UniformRandom));
            UniformRandom ur = new UniformRandom(2, 0.3456f);
            
            Stream str = new MemoryStream(9999);
            serializer.Serialize(str, ur);
            StreamReader reader = new StreamReader(str);
            str.Seek(0, SeekOrigin.Begin);
            string result1 = reader.ReadToEnd();
            
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineC
            var xmlWriter = XmlWriter.Create(sb);
            serializer.Serialize(xmlWriter, ur);
            string result2 = sb.ToString();
        }*/
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