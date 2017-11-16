using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions.Triggers
{
    /// <summary>
    /// This action trigger return true if a particle is dead.
    /// </summary>
    public class DeathTrigger : Trigger
    {
        
        public override bool TestTrigger(Emitter emitter, Particle particle, float time)
        {
            return particle.IsDead;
        }

    }
}