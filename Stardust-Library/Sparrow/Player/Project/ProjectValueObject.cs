using System.Collections.Generic;
using Stardust.Sparrow.Player.Emitters;

namespace Stardust.Sparrow.Player.Project
{
    public class ProjectValueObject
    {
        public float Version;

        public readonly IList<EmitterValueObject> Emitters = new List<EmitterValueObject>();

        public ProjectValueObject(float version)
        {
            Version = version;
        }

        public int NumberOfEmitters => Emitters.Count;

        public int NumberOfParticles
        {
            get
            {
                int num = 0;
                foreach (var emitterVo in Emitters)
                {
                    num += emitterVo.Emitter.NumParticles;
                }
                return num;
            }
        }

        /// <summary>
        /// Removes all particles and puts the simulation back to its initial state.
        /// </summary>
        public void ResetSimulation()
        {
            foreach (var emitterVo in Emitters)
            {
                emitterVo.Emitter.Reset();
            }
        }

        public float Fps
        {
            get => Emitters[0].Emitter.Fps;
            set
            {
                foreach (var emitterVo in Emitters)
                {
                    emitterVo.Emitter.Fps = value;
                }
            }
        }

        public void Destroy()
        {
            foreach (var emitterVo in Emitters)
            {
                var emitter = emitterVo.Emitter;
                emitter.ClearParticles();
                emitter.ClearActions();
                emitter.CleaInitializers();
                emitterVo.EmitterSnapshot = null;
                SparrowRenderer renderer = ((StarlingHandler)(emitter.ParticleHandler)).Renderer;
                if (renderer.Parent != null)
                {
                    renderer.RemoveFromParent();
                }
            }
            Emitters.Clear();
        }
        
        
    }
}