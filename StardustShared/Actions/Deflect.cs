using System.Collections.Generic;
using Stardust.Actions.Deflectors;
using Stardust.Emitters;
using Stardust.Geom;
using Stardust.Particles;

namespace Stardust.Actions
{
    /// <summary>
    /// This action is useful to manipulate a particle's position and velocity as you like.
    ///
    /// <para>
    /// Each deflector returns a <code>Vec4D</code> object, which contains four numeric properties: x, y, vx, and vy,
    /// according to the particle's position and velocity.
    /// The particle's position and velocity are then reassigned to the new values (x, y) and (vx, vy), respectively.
    /// </para>
    ///
    /// <para>
    /// Deflectors can be used to create obstacles, bounding boxes, etc.
    /// </para>
    /// </summary>
    public class Deflect : Action
    {

        public List<Deflector> Deflectors;

        public Deflect()
        {
            _priority = -5;
            Deflectors = new List<Deflector>();
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            foreach (Deflector deflector in Deflectors)
            {
                var md4D = deflector.GetMotionData4D(particle);
                if (md4D != null)
                {
                    particle.IsDeflected = true;
                    particle.X = md4D.X;
                    particle.Y = md4D.Y;
                    particle.Vx = md4D.Vx;
                    particle.Vy = md4D.Vy;
                    Vec4D.RecycleToPool(md4D);
                }
                else
                {
                    particle.IsDeflected = false;
                }
            }
        }
    }
}