using Sparrow.Display;
using Stardust.Sparrow.Player.Emitters;
using Stardust.Sparrow.Player.Project;

namespace Stardust.Sparrow.Player
{
    /// <summary>
    /// Simple class to play back simulations.
    /// </summary>
    public class SimPlayer
    {
        private ProjectValueObject _project;
        public ProjectValueObject Project => _project;

        public SimPlayer(ProjectValueObject project, DisplayObjectContainer renderTarget)
        {
            SetProject(project, renderTarget);
        }
        
        public void SetProject(ProjectValueObject project, DisplayObjectContainer renderTarget)
        {
            _project = project;
            foreach (var emitterValueObject in project.Emitters)
            {
                StarlingHandler handler = (StarlingHandler) emitterValueObject.Emitter.ParticleHandler;
                handler.Container = renderTarget;
            }
        }

        public void StepSimulation(float deltaTime)
        {
            foreach (EmitterValueObject emitterValueObject in _project.Emitters)
            {
                emitterValueObject.Emitter.Step(deltaTime);
            }
        }
        
    }
}