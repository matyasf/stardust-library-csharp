
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Sparrow.Core;
using Sparrow.Display;
using Sparrow.Utils;
using Stardust.Actions;
using Stardust.Clocks;
using Stardust.Emitters;
using Stardust.Handlers.Sparrow;
using Stardust.Initializers;
using Stardust.Math;
using Stardust.Xml;
using Stardust.Zones;

namespace Stardust_Library_Sample
{
    public class SampleApp : Sprite
    {
        private Emitter em;
        
        public SampleApp()
        {
            SparrowSharp.EnableErrorChecking();
            SparrowSharp.SkipUnchangedFrames = false;
            SparrowSharp.ShowStats(HAlign.Right);
            EnterFrame += OnEnterFrame;
            
            Stream stream = Assembly.GetExecutingAssembly().
                GetManifestResourceStream("Stardust_Library_Sample.emitter_simple.xml");
            
            XElement elem = XElement.Load(stream);
            
            XmlBuilder builder = new XmlBuilder();
            builder.RegisterClass(typeof(Age));
            builder.RegisterClass(typeof(Life));
            builder.RegisterClass(typeof(UniformRandom));
            builder.RegisterClass(typeof(SteadyClock));
            builder.RegisterClass(typeof(Emitter));
            builder.RegisterClass(typeof(SimpleSparrowHandler));
            builder.RegisterClass(typeof(SparrowHandler));
            builder.RegisterClass(typeof(DeathLife));
            builder.RegisterClass(typeof(Velocity));
            builder.RegisterClass(typeof(Move));
            builder.RegisterClass(typeof(SinglePoint));
            builder.BuildFromXml(elem);
            var result = builder.GetElementByName("0");
            em = (Emitter) result;
            
            var handler = (SimpleSparrowHandler)em.ParticleHandler;
            var sp = new Sprite();
            sp.X = sp.Y = 100;
            AddChild(sp);
            handler.Container = sp;
        }
        
        private void OnEnterFrame(DisplayObject target, float passedTime)
        {
            em.Step(passedTime / 1000f);
            Debug.WriteLine(em.NumParticles);
        }
    }
}