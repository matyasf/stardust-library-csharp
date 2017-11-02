using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using Stardust.Actions;
using Stardust.Clocks;
using Stardust.Emitters;
using Stardust.Handlers;
using Stardust.Initializers;
using Stardust.Math;
using Stardust.Xml;

namespace TestApp
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            Debug.WriteLine("works");
            
            var stream = Assembly.GetExecutingAssembly().
                GetManifestResourceStream("TestApp.emitter_simple.xml");
            
            XElement elem = XElement.Load(stream);
            
            XmlBuilder builder = new XmlBuilder();
            builder.RegisterClass(typeof(Age));
            builder.RegisterClass(typeof(Life));
            builder.RegisterClass(typeof(UniformRandom));
            builder.RegisterClass(typeof(SteadyClock));
            builder.RegisterClass(typeof(Emitter));
            builder.RegisterClass(typeof(SparrowHandler));
            builder.BuildFromXml(elem);
            var result = builder.GetElementByName("0");
            Emitter em = (Emitter) result;
            var ac = em.Actions;
            var init = em.Initializers;
        }
    }
}