
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Sparrow.Core;
using Sparrow.Display;
using Sparrow.Text;
using Sparrow.Textures;
using Sparrow.Utils;
using Stardust.Actions;
using Stardust.Clocks;
using Stardust.Emitters;
using Stardust.Handlers.Sparrow;
using Stardust.Handlers.Sparrow.simple;
using Stardust.Initializers;
using Stardust.MathStuff;
using Stardust.Xml;
using Stardust.Zones;

namespace Stardust_Library_Sample
{
    public class SampleApp : Sprite
    {
        private Emitter em;
        private float timeCounter;
        private int frameNum;
        private TextField textField;
        
        public SampleApp()
        {
            //SparrowSharp.EnableErrorChecking();
            SparrowSharp.SkipUnchangedFrames = false;
            //SparrowSharp.ShowStats(HAlign.Right);
            
            SparrowRenderer.Init(1, 35000);
            timeCounter = 0;
            Stream stream = Assembly.GetExecutingAssembly().
                //GetManifestResourceStream("Stardust_Library_Sample.emitter_simple.xml");
                GetManifestResourceStream("Stardust_Library_Sample.emitter_perf_renderer.xml");
            
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
            builder.RegisterClass(typeof(Alpha));
            builder.RegisterClass(typeof(SinglePoint));
            builder.RegisterClass(typeof(Line));
            builder.RegisterClass(typeof(PositionAnimated));
            builder.BuildFromXml(elem);
            var result = builder.GetElementByName("0");
            em = (Emitter) result;
            
            var sp = new Sprite();
            sp.X = sp.Y = 100;
            AddChild(sp);
            
            if (em.ParticleHandler is SimpleSparrowHandler)
            {
                var handler = (SimpleSparrowHandler)em.ParticleHandler; 
                handler.Container = sp;
            }
            else
            {
                var handler = (SparrowHandler)em.ParticleHandler; 
                handler.Container = sp;
                
                var subTex = new SubTexture(Texture.FromColor(4, 4, 0xFF33FF));
                var subTexes = new List<SubTexture>();
                subTexes.Add(subTex);
                handler.Textures = subTexes;
            }
            
            textField = new TextField(150, 40);
            AddChild(textField);
            EnterFrame += OnEnterFrame;
        }
        
        private void OnEnterFrame(DisplayObject target, float passedTime)
        {
            em.Step(passedTime / 1000f);
            timeCounter = timeCounter + passedTime;
            frameNum++;
            if (timeCounter > 500)
            {
                textField.Text = "Fps: " + (frameNum * 2) + " #p: " + em.NumParticles;
                timeCounter = 0;
                frameNum = 0;
            }
        }
    }
}