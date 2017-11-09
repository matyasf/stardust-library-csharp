
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Sparrow.Core;
using Sparrow.Display;
using Sparrow.Text;
using Sparrow.Textures;
using Stardust.Actions;
using Stardust.Clocks;
using Stardust.Emitters;
using Stardust.Initializers;
using Stardust.MathStuff;
using Stardust.Sparrow;
using Stardust.Sparrow.Player;
using Stardust.Xml;
using Stardust.Zones;

namespace Stardust_Library_Sample
{
    public class SampleApp : Sprite
    {
        private readonly Emitter _em;
        private float _timeCounter;
        private int _frameNum;
        private readonly TextField _textField;
        
        public SampleApp()
        {
            //SparrowSharp.EnableErrorChecking();
            SparrowSharp.SkipUnchangedFrames = false;
            //SparrowSharp.ShowStats(HAlign.Right);
            
            SparrowRenderer.Init(1, 35000);
            _timeCounter = 0;
            
            var sp = new Sprite();
            sp.X = sp.Y = 100;
            AddChild(sp);

            var resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            Stream sdeStream = Assembly.GetExecutingAssembly().
                GetManifestResourceStream("Stardust_Library_Sample.Untitled.zip");
            SimLoader loader = new SimLoader();
            loader.LoadSim(sdeStream);
            var sim = loader.CreateProjectInstance();
            var player = new SimPlayer(sim, sp);

            EnterFrame += (target, time) =>
            {
                player.StepSimulation(time/1000);
            };
            /*
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
            _em = (Emitter) result;
            
            if (_em.ParticleHandler is SimpleSparrowHandler)
            {
                var handler = (SimpleSparrowHandler)_em.ParticleHandler; 
                handler.Container = sp;
            }
            else
            {
                var handler = (SparrowHandler)_em.ParticleHandler; 
                handler.Container = sp;
                
                var subTex = new SubTexture(Texture.FromColor(4, 4, 0xFF33FF));
                var subTexes = new List<SubTexture>();
                subTexes.Add(subTex);
                handler.Textures = subTexes;
            }
            
            _textField = new TextField(150, 40);
            AddChild(_textField);
            EnterFrame += OnEnterFrame;
            */
        }
        
        private void OnEnterFrame(DisplayObject target, float passedTime)
        {
            _em.Step(passedTime / 1000f);
            _timeCounter = _timeCounter + passedTime;
            _frameNum++;
            if (_timeCounter > 500)
            {
                _textField.Text = "Fps: " + (_frameNum * 2) + " #p: " + _em.NumParticles;
                _timeCounter = 0;
                _frameNum = 0;
            }
        }
    }
}