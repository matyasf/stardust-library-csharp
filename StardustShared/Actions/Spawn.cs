using Stardust.Actions.Triggers;
using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions
{
    /// <summary>
    /// Spawns new particles at the position of existing particles.
    /// This action can be used to create effects such as fireworks, rocket trails, etc.
    ///
    /// You must specify an emitter that will emit the new particles. This action offsets the emitters newly created
    /// particles position to the position this emitters particles.
    /// You should set the spawner emitter's active property to false so it does not emit particles by itself.
    /// Furthermore to spawn particles you need to add a trigger to this action.
    /// </summary>
    public class Spawn : Action
    {

        public bool InheritDirection;
        public bool InheritVelocity;
        private Emitter _spawnerEmitter;
        private Trigger _trigger;
        
        public Spawn() : this(true, true, null) {}
        
        public Spawn(bool inheritDirection, bool inheritVelocity, Trigger trigger)
        {
            Priority = -10;
            InheritDirection = inheritDirection;
            InheritVelocity = inheritVelocity;
            Trigger = trigger;
        }
        
        public Emitter SpawnerEmitter
        {
            get => _spawnerEmitter;
            set
            {
                _spawnerEmitter = value;
                SpawnerEmitterId = value?.Name;
            }
        }

        public string SpawnerEmitterId { get; set; }

        public Trigger Trigger
        {
            get => _trigger;
            set
            {
                if (value == null)
                {
                    value = new DeathTrigger();
                }
                _trigger = value;
            }
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            if (_spawnerEmitter == null) 
            {
                return;
            }
            if (_trigger.TestTrigger(emitter, particle, timeDelta)) 
            {
                var newParticles = _spawnerEmitter.CreateParticles(_spawnerEmitter.Clock.GetTicks(timeDelta));
                var len = newParticles.Count;
                for (int m  = 0; m < len; ++m) {
                    var p = newParticles[m];
                    p.X += particle.X;
                    p.Y += particle.Y;
                    if (InheritVelocity) {
                        p.Vx += particle.Vx;
                        p.Vy += particle.Vy;
                    }
                    if (InheritDirection) {
                        p.Rotation += particle.Rotation;
                    }
                }
            }
        }
        
    }
}