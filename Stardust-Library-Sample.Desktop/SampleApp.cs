using System.IO;
using System.Reflection;
using Sparrow.Core;
using Sparrow.Display;
using Sparrow.Text;
using Sparrow.Textures;
using Sparrow.Utils;
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
        private RenderTexture _renderTexture;
        private Sprite _simContainer;
        
        public SampleApp()
        {
            SparrowSharp.Stage.Color = 0x343434;
            //SparrowSharp.EnableErrorChecking();
            SparrowSharp.SkipUnchangedFrames = false;
            SparrowSharp.ShowStats(HAlign.Right);
            
            SparrowRenderer.Init(1, 35000);

            EnterFrame += SampleApp_EnterFrame;
            
            _renderTexture = new RenderTexture(500, 500, false);
            var q = new Quad(500, 500);
            q.X = 100;
            q.Y = 100;
            q.Texture = _renderTexture;
            AddChild(q);
            
            _simContainer = new Sprite();
            _simContainer.X = _simContainer.Y = 250;

        }

        private void SampleApp_EnterFrame(DisplayObject target, float passedTime)
        {
            if (cnt == 0)
            {
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
                var loadeds = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                Stream sdeStream = Assembly.GetExecutingAssembly().
                    GetManifestResourceStream("Stardust-Library-Sample.snapshot.sde");
                SimLoader loader = new SimLoader();
                loader.LoadSim(sdeStream);
                var sim = loader.CreateProjectInstance();
                player = new SimPlayer(sim, _simContainer);
                
            }
            else
            {
                player.StepSimulation(passedTime);
                _renderTexture.Draw(_simContainer);
            }
            cnt++;
        }

        int cnt = 0;
    }

}