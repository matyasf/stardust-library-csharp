using System;
using System.Collections.Generic;
using Sparrow.Textures;
using Stardust.Emitters;

namespace Stardust.Sparrow.Player.Emitters
{
    public class EmitterValueObject
    {
        public Emitter2D Emitter;
        
        /// <summary>
        /// Snapshot of the particles. If its not null then the emitter will have the particles stored here upon creation.
        /// </summary>
        public string EmitterSnapshot; // TODO

        public EmitterValueObject(Emitter2D _emitter)
        {
            Emitter = _emitter;
        }

        public string Id => Emitter.Name;

        public IList<SubTexture> Textures => (Emitter.ParticleHandler as StarlingHandler).Textures;

        public void AddParticlesFromSnapshot()
        {
            throw new NotImplementedException();// todo
        }
    }
}