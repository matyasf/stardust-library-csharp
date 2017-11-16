using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions.Triggers
{
    public abstract class Trigger : StardustElement
    {
        public abstract bool TestTrigger(Emitter emitter, Particle particle, float time);
        
    }
}