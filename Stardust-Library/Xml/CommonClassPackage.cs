using Stardust.Actions;
using Stardust.Actions.Triggers;
using Stardust.Clocks;
using Stardust.Emitters;
using Stardust.Initializers;
using Stardust.MathStuff;
using Stardust.Zones;

namespace Stardust.Xml
{
    public class CommonClassPackage : ClassPackage
    {
        private static CommonClassPackage _instance;

        public static CommonClassPackage GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CommonClassPackage();
            }
            return _instance;
        }
        
        private CommonClassPackage() {}
        
        protected override void PopulateClasses()
        {
            //actions
            classes.Add(typeof(Age));
            classes.Add(typeof(DeathLife));
            classes.Add(typeof(Move));
            classes.Add(typeof(Spawn));
            classes.Add(typeof(ColorGradient));
            classes.Add(typeof(NormalDrift));
            classes.Add(typeof(Spin));
           
            //action triggers
            classes.Add(typeof(DeathTrigger));
            
            //deflectors

            //fields

            //clocks
            classes.Add(typeof(SteadyClock));
            classes.Add(typeof(ImpluseClock));

            //emitters
            classes.Add(typeof(Emitter2D));

            //initializers
            classes.Add(typeof(Alpha));
            classes.Add(typeof(Life));
            classes.Add(typeof(Velocity));
            classes.Add(typeof(PositionAnimated));
            classes.Add(typeof(Mass));
            classes.Add(typeof(Omega));
            classes.Add(typeof(Rotation));
            classes.Add(typeof(Scale));

            //randoms
            classes.Add(typeof(UniformRandom));

            //zones
            classes.Add(typeof(Line));
            classes.Add(typeof(RectZone));
            classes.Add(typeof(SinglePoint));

        }
    }
}