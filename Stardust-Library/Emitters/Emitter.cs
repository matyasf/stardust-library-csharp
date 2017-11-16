using System.Collections.Generic;
using Newtonsoft.Json;
using Stardust.Actions;
using Stardust.Clocks;
using Stardust.Collections;
using Stardust.Handlers;
using Stardust.Initializers;
using Stardust.Particles;

namespace Stardust.Emitters
{
    /// <summary>
    /// This class takes charge of the actual particle simulation of the Stardust particle system.
    /// </summary>
    public class Emitter : StardustElement
    {
        
        public delegate void EmitterStepEndHandler(Emitter emitter);
        public event EmitterStepEndHandler EmitterStepEnd;
        
        private readonly List<Particle> _particles = new List<Particle>(20);


        /// <summary>
        /// Returns every managed particle for custom parameter manipulation.
        /// The returned List is not a copy.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<Particle> Particles => _particles;

        /// <summary>
        /// Particle handler is used to render particles.
        /// </summary>
        public ParticleHandler ParticleHandler;
        
        /// <summary>
        /// The clock determines when to spawn particles
        /// </summary>
        private Clock _clock;

        /// <summary>
        /// Whether the emitter is active, true by default.
        /// <para>
        /// If the emitter is active, it creates particles in each step according to its clock.
        /// Note that even if an emitter is not active, the simulation of existing particles still goes on in each step.
        /// </para>
        /// </summary>
        public bool Active;

        /// <summary>
        /// The time since the simulation is running.
        /// </summary>
        [JsonIgnore]
        public float CurrentTime;

        /// <summary>
        /// While the max. fps is usually 60, the actual value fluctuates a few ms.
        /// Thus using the real value would cause lots of frame skips
        /// To take this into account Stardust uses internally a slightly smaller value to compensate.
        /// </summary>
        public const float TimeStepCorrectionOffset = 0.004f;
        private readonly PooledParticleFactory _factory = new PooledParticleFactory();
        private readonly SortableCollection<Action> _actionCollection = new SortableCollection<Action>();
        private readonly List<Action> _activeActions = new List<Action>();
        private float _invFps = 1f / 60f - TimeStepCorrectionOffset;
        private float _timeSinceLastStep;
        private float _fps;

        public Emitter() : this(null, null) {}
        
        public Emitter(Clock clock, ParticleHandler particleHandler)
        {
            Clock = clock;
            Active = true;
            ParticleHandler = particleHandler;
            Fps = 60f;
        }

        /// <summary>
        /// The clock determines how many particles the emitter creates in each step.
        /// </summary>
        public Clock Clock
        {
            get => _clock;
            set
            {
                if (value == null)
                {
                    value = new SteadyClock(0f);
                }
                _clock = value;
            }
        }
        
        /// <summary>
        /// Sets the frame rate of the simulation. Lower framerates consume less CPU, but make your animations
        /// look choppy. Note that the simulation behaves slightly differently at different FPS settings
        /// (e.g. A clock produces the same amount of ticks on all FPSes, but it does it at a different times,
        /// resulting in particles emitted in batches instead smoothly)
        /// </summary>
        public float Fps
        {
            get => _fps;
            set
            {
                if (value > 60)
                {
                    value = 60;
                }
                _fps = value;
                _invFps = 1 / value - TimeStepCorrectionOffset;
            }
        }
        
        /// <summary>
        /// Resets all properties to their default values and removes all particles.
        /// </summary>
        public void Reset()
        {
            CurrentTime = 0;
            ClearParticles();
            _clock.Reset();
            ParticleHandler.Reset();
        }

        #region main loop

        /// <summary>
        /// This method is the main simulation loop of the emitter.
        ///
        /// <para>
        /// In order to keep the simulation go on, this method should be called continuously.
        /// It is recommended that you call this method through the <code>Event.ENTER_FRAME</code> event or the <code>TimerEvent.TIMER</code> event.
        /// </para>
        /// </summary>
        /// <param name="time">The time elapsed since the last step in milliseconds</param>
        public void Step(float time)
        {
            if (time <= 0)
            {
                return;
            }
            time = time / 1000;
            _timeSinceLastStep = _timeSinceLastStep + time;
            CurrentTime = CurrentTime + time;
            if (_timeSinceLastStep < _invFps)
            {
                return;
            }
            ParticleHandler.StepBegin(this, _particles, _timeSinceLastStep);
            
            if (Active) {
                CreateParticles(_clock.GetTicks(_timeSinceLastStep));
            }
            
            //filter out active actions
            _activeActions.Clear();
            foreach (Action action in Actions)
            {
                if (action.Active) {
                    _activeActions.Add(action);
                }
            }
            
            // sorting
            foreach (Action activeAction in _activeActions)
            {
                if (activeAction.NeedsSortedParticles)
                {
                    _particles.Sort();
                    break;
                }
            }
            //invoke action preupdates
            foreach (Action action in _activeActions)
            {
                action.PreUpdate(this, _timeSinceLastStep);
            }
            //update the remaining particles
            List<Particle> deadParticles = new List<Particle>(); // do not instantiate here
            foreach (Particle particle in _particles)
            {
                foreach (Action activeAction in _activeActions)
                {
                    activeAction.Update(this, particle, _timeSinceLastStep, CurrentTime);
                }

                if (particle.IsDead)
                {
                    deadParticles.Add(particle);
                }
            }
            foreach (Particle deadParticle in deadParticles)
            {
                ParticleHandler.ParticleRemoved(deadParticle);
                    
                deadParticle.Destroy();
                _factory.Recycle(deadParticle);

                _particles.Remove(deadParticle);
            }
            
            // postUpdate
            foreach (Action activeAction in _activeActions)
            {
                activeAction.PostUpdate(this, _timeSinceLastStep);
            }

            EmitterStepEnd?.Invoke(this);
            
            ParticleHandler.StepEnd(this, _particles, _timeSinceLastStep);

            _timeSinceLastStep = 0;
        }
        
        #endregion

        #region actions & initializers

        /// <summary>
        /// Holds every action for this emitter.
        /// </summary>
        public List<Action> Actions
        {
            get => _actionCollection.Elems;
            set
            {
                foreach (Action element in value)
                {
                    AddAction(element);
                }
            }
            
        } 

        /// <summary>
        /// Adds an action to the emitter.
        /// </summary>
        public void AddAction(Action action)
        {
            _actionCollection.Add(action);
            action.DispatchAddEvent();
        }
        
        /// <summary>
        /// Removes an action from the emitter.
        /// </summary>
        public void RemoveAction(Action action)
        {
            _actionCollection.Remove(action);
            action.DispatchRemoveEvent();
        }
        
        /// <summary>
        /// Removes all actions from the emitter.
        /// </summary>
        public void ClearActions()
        {
            var actions = _actionCollection.Elems;
            var len = actions.Count;
            for (int i = 0; i < len; ++i) 
            {
                actions[i].DispatchRemoveEvent();
            }
            _actionCollection.Clear();
        }

        /// <summary>
        /// Holds every initializer for this emitter.
        /// </summary>
        public List<Initializer> Initializers
        {
            get => _factory.InitializerCollection.Elems;
            set
            {
                foreach (Initializer element in value)
                {
                    AddInitializer(element);
                }
            }
        }

        /// <summary>
        /// Adds an initializer to the emitter.
        /// </summary>
        public void AddInitializer(Initializer initializer)
        {
            _factory.AddInitializer(initializer);
            initializer.DispatchAddEvent();
        }
        
        /// <summary>
        /// Reomves an initializer from the emitter.
        /// </summary>
        public void RemoveInitializer(Initializer initializer)
        {
            _factory.RemoveInitializer(initializer);
            initializer.DispatchRemoveEvent();
        }
        
        /// <summary>
        /// Removes all initializers from the emitter.
        /// </summary>
        public void CleaInitializers()
        {
            var initializers = _factory.InitializerCollection.Elems;
            var len = initializers.Count;
            for (int i = 0; i < len; ++i) {
                initializers[i].DispatchRemoveEvent();
            }
            _factory.ClearInitializers();
        }
        
        #endregion

        #region particles

        /// <summary>
        /// The number of particles in the emitter.
        /// </summary>
        [JsonIgnore]
        public int NumParticles => _particles.Count;
        
        private static readonly List<Particle> NewParticles = new List<Particle>();
        /// <summary>
        /// This method is called by the emitter to create new particles.
        /// </summary>
        public IList<Particle> CreateParticles(int pCount)
        {
            NewParticles.Clear();
            _factory.CreatParticles(pCount, CurrentTime, NewParticles);
            AddParticles(NewParticles);
            return NewParticles;
        }
        
        /// <summary>
        /// This method is used to manually add existing particles to the emitter's simulation.
        /// Note: you have to initialize the particles manually! To call all initializers in this emitter for the
        /// particle use <code>CreateParticles()</code> instead.
        /// </summary>
        public void AddParticles(IEnumerable<Particle> particles)
        {
            foreach (Particle particle in particles)
            {
                _particles.Add(particle);
                //handle adding
                ParticleHandler.ParticleAdded(particle);
            }
        }
        
        /// <summary>
        /// Clears all particles from the emitter's simulation.
        /// </summary>
        public void ClearParticles()
        {
            foreach (Particle particle in _particles)
            {
                ParticleHandler.ParticleRemoved(particle);
                particle.Destroy();
                _factory.Recycle(particle);
            }
            _particles.Clear();
        }

        #endregion
        
    }
}