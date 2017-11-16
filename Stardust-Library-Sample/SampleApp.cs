using System.IO;
using System.Reflection;
using Sparrow.Core;
using Sparrow.Display;
using Sparrow.Text;
using Stardust.Emitters;
using Stardust.Sparrow;
using Stardust.Sparrow.Player;

namespace Stardust_Library_Sample
{
    public class SampleApp : Sprite
    {
        private readonly Emitter _em;
        private int _frameNum;
        private readonly TextField _textField;
        private SimPlayer player;
        
        public SampleApp()
        {
            SparrowSharp.Stage.Color = 0x343434;
            //SparrowSharp.EnableErrorChecking();
            SparrowSharp.SkipUnchangedFrames = false;
            //SparrowSharp.ShowStats(HAlign.Right);
            
            SparrowRenderer.Init(1, 35000);

            EnterFrame += SampleApp_EnterFrame;
        }

        private void SampleApp_EnterFrame(DisplayObject target, float passedTime)
        {
            if (cnt == 0)
            {
                var sp = new Sprite();
                sp.X = sp.Y = 300;
                AddChild(sp);
                /*
                Emitter emm = new Emitter(new SteadyClock(23, new UniformRandom(34, 22)), new StarlingHandler());
                emm.AddInitializer(new Life(new UniformRandom(4, 44)));
                emm.AddInitializer(new PositionAnimated(new List<Zone>(){new SinglePoint(), new Line()}));
                emm.AddAction(new ColorGradient(true));
                emm.ParticleHandler = new StarlingHandler();
                
                StardustSerializer zer = new StardustSerializer();
                var json = zer.Serialize(emm);
                var newEm = zer.Deserialize(json);
                int a = 5;
                */
                Stream sdeStream = Assembly.GetExecutingAssembly().
                    GetManifestResourceStream("Stardust_Library_Sample.blazingFire.sde");
                SimLoader loader = new SimLoader();
                loader.LoadSim(sdeStream);
                var sim = loader.CreateProjectInstance();
                player = new SimPlayer(sim, sp);
                
            }
            else
            {
                player.StepSimulation(passedTime);
            }
            cnt++;
        }

        int cnt = 0;
    }

}