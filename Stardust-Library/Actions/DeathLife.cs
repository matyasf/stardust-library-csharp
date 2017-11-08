using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions
{
    public class DeathLife : Action
    {

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            if (particle.Life <= 0)
            {
                particle.IsDead = true;
            }
        }
        
        #region XML

        public override string GetXmlTagName()
        {
            return "DeathLife";
        }

        #endregion
    }
}