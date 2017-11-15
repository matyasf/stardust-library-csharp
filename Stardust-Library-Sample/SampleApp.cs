
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Sparrow.Core;
using Sparrow.Display;
using Sparrow.Text;
using Stardust;
using Stardust.Actions;
using Stardust.Clocks;
using Stardust.Emitters;
using Stardust.Initializers;
using Stardust.MathStuff;
using Stardust.Serialization;
using Stardust.Sparrow;
using Stardust.Sparrow.Player;
using Stardust.Zones;

namespace Stardust_Library_Sample
{
    public class SampleApp : Sprite
    {
        private readonly Emitter2D _em;
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
                GetManifestResourceStream("Stardust_Library_Sample.Untitled.zip");
            SimLoader loader = new SimLoader();
            loader.LoadSim(sdeStream);
            var sim = loader.CreateProjectInstance();
            var player = new SimPlayer(sim, sp);

            EnterFrame += (target, time) =>
            {
                player.StepSimulation(time);
            };
            
            Emitter2D em = sim.Emitters[0].Emitter;
            
           
            var err = new NormalDrift(123, new UniformRandom(12,333));
            
            StardustSerializer zer = new StardustSerializer();
            var json = zer.Serialize(em);
            var szar = zer.Deserialize(json);
            
            int a = 45;   
        }
    }

}