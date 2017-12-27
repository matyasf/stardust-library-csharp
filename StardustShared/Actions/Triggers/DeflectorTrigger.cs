
using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions.Triggers
{
    public class DeflectorTrigger : Trigger
    {
        
        public override bool TestTrigger(Emitter emitter, Particle particle, float time)
        {
            return particle.IsDeflected;
        }
    }
}