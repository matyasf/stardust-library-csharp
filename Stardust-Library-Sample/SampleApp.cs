
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Sparrow.Core;
using Sparrow.Display;
using Sparrow.Text;
using Stardust.Actions;
using Stardust.Clocks;
using Stardust.Emitters;
using Stardust.Initializers;
using Stardust.MathStuff;
using Stardust.Sparrow;
using Stardust.Sparrow.Player;
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
            SparrowSharp.Stage.Color = 0x343434;
            //SparrowSharp.EnableErrorChecking();
            SparrowSharp.SkipUnchangedFrames = false;
            //SparrowSharp.ShowStats(HAlign.Right);
            
          //  ColorGradient cg = new ColorGradient();
            
           // cg.SetGradient(new uint[] {0x000000, 0x10FFFF, 0xFFFFFF}, new float[] {0, 128, 255}, new float[] {1, 0, 1});
            

            
            SparrowRenderer.Init(1, 35000);
            _timeCounter = 0;
            
            var sp = new Sprite();
            sp.X = sp.Y = 300;
            AddChild(sp);
            
            Stream sdeStream = Assembly.GetExecutingAssembly().
                //GetManifestResourceStream("Stardust_Library_Sample.Untitled.zip");
                GetManifestResourceStream("Stardust_Library_Sample.blazingFire.zip");
            SimLoader loader = new SimLoader();
            loader.LoadSim(sdeStream);
            var sim = loader.CreateProjectInstance();
            var player = new SimPlayer(sim, sp);

            EnterFrame += (target, time) =>
            {
                player.StepSimulation(time);
            };
            
            Emitter em = sim.Emitters[0].Emitter;
            Type[] extras =
            {
                typeof(UniformRandom), 
                
                typeof(SteadyClock), 
                typeof(ImpluseClock),
                
                typeof(SinglePoint),
                typeof(Line),
                typeof(RectZone),
                
                typeof(SparrowHandler),
                
                typeof(DeathLife),
                typeof(Age),
                typeof(ColorGradient),
                typeof(Move),
                typeof(NormalDrift),
                typeof(Spin),
                
                typeof(Alpha),
                typeof(Life),
                typeof(Mass),
                typeof(Omega),
                typeof(PositionAnimated),
                typeof(Rotation),
                typeof(Scale),
                typeof(Velocity),
            };
            
            var ee = new NormalDrift(32, new UniformRandom(5, 2));
            
            
            var serializer = new XmlSerializer(typeof(Emitter), extras);
            StringBuilder sb = new StringBuilder();
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            var xtw = XmlWriter.Create(sb, settings);
            serializer.Serialize(xtw, em);
            xtw.Flush();
            var xstr = sb.ToString();
            
            int a = 45;
            
        }
        
    }

}