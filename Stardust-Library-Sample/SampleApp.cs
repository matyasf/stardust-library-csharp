
using System.IO;
using System.Reflection;
using Sparrow.Core;
using Sparrow.Display;
using Sparrow.Text;
using Stardust.Actions;
using Stardust.Emitters;
using Stardust.Sparrow;
using Stardust.Sparrow.Player;

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
            
            ColorGradient cg = new ColorGradient();
            
            cg.SetGradient(new uint[] {0x000000, 0x10FFFF, 0xFFFFFF}, new float[] {0, 128, 255}, new float[] {1, 0, 1});
            
            SparrowRenderer.Init(1, 35000);
            _timeCounter = 0;
            
            var sp = new Sprite();
            sp.X = sp.Y = 300;
            AddChild(sp);
            
            Stream sdeStream = Assembly.GetExecutingAssembly().
                //GetManifestResourceStream("Stardust_Library_Sample.Untitled.zip");
                GetManifestResourceStream("Stardust_Library_Sample.Untitled2.sde");
            SimLoader loader = new SimLoader();
            loader.LoadSim(sdeStream);
            var sim = loader.CreateProjectInstance();
            var player = new SimPlayer(sim, sp);

            EnterFrame += (target, time) =>
            {
                player.StepSimulation(time);
            };
        }
        
    }
}