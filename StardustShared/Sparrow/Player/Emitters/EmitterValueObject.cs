
using System.Collections.Generic;
using Sparrow.Textures;
using Stardust.Emitters;
using Stardust.Particles;
using StardustProtos;

namespace Stardust.Sparrow.Player.Emitters
{
    public class EmitterValueObject
    {
        public Emitter Emitter;
        
        /// <summary>
        /// Snapshot of the particles. If its not null then the emitter will have the particles stored here upon creation.
        /// </summary>
        public IList<ParticleSnapshot> EmitterSnapshot;

        public EmitterValueObject(Emitter _emitter)
        {
            Emitter = _emitter;
        }

        public string Id => Emitter.Name;

        public IList<SubTexture> Textures => (Emitter.ParticleHandler as StarlingHandler).Textures;

        public void AddParticlesFromSnapshot()
        {
            var particles = new List<Particle>();
            foreach (var snapshot in EmitterSnapshot)
            {
                var p = new Particle();
                //p.CollisionRadius = snapshot.??
                p.Alpha = snapshot.Alpha;
                p.ColorB = snapshot.ColorB;
                p.ColorG = snapshot.ColorG;
                p.ColorR = snapshot.ColorR;
                p.CurrentAnimationFrame = snapshot.CurrentAnimationFrame;
                p.InitLife = snapshot.InitLife;
                p.IsDead = snapshot.IsDead;
                p.Life = snapshot.Life;
                p.Mass = snapshot.Mass;
                p.Omega = snapshot.Omega;
                p.Rotation = snapshot.Rotation;
                p.Scale = snapshot.Scale;
                p.Vx = snapshot.Vx;
                p.Vy = snapshot.Vy;
                p.X = snapshot.X;
                p.Y = snapshot.Y;
                particles.Add(p);
            }
            Emitter.AddParticles(particles);
        }
    }
}