using System.Runtime.Serialization;
using Stardust.MathStuff;

namespace Stardust.Clocks
{
    /// <summary>
    /// This clock can be used to create randomized impulses and has more parameters than SteadyClock
    /// </summary>
    public class ImpluseClock : Clock
    {

        private RandomBase _impulseInterval;

        /// <summary>
        /// How many particles to create when an impulse is happening.
        /// </summary>
        public float TicksPerCall;

        private RandomBase _initialDelay;
        private float _currentImpulseInterval;
        private float _currentImpulseLength;
        private float _currentInitialDelay;
        private RandomBase _impulseLength;
        private float _currentTime;
        
        /// <summary>
        /// The delay in seconds until the first impulse happens.
        /// </summary>
        public RandomBase InitialDelay
        {
            get => _initialDelay;
            set
            {
                _initialDelay = value;
                SetCurrentInitialDelay();
            }
        }
        
        /// <summary>
        /// The length of a impulses in seconds.
        /// </summary>
        public RandomBase ImpulseLength
        {
            get => _impulseLength;
            set
            {
                _impulseLength = value;
                SetCurrentImpulseLength();
            }
        }
        
        /// <summary>
        /// The time between a impulses in seconds.
        /// </summary>
        public RandomBase ImpulseInterval
        {
            get => _impulseInterval;
            set
            {
                _impulseInterval = value;
                SetCurrentImpulseInterval();
            }
        }
        
        public ImpluseClock() : this(null, null, null, 1) {}
        
        public ImpluseClock(RandomBase impulseInterval,
                            RandomBase impulseLength,
                            RandomBase initialDelay,
                            float ticksPerCall)
        {
            ImpulseInterval = impulseInterval == null ? new UniformRandom(20, 10) : impulseInterval;
            ImpulseLength = impulseLength == null ? new UniformRandom(5, 0) : impulseLength;
            InitialDelay = initialDelay == null ? new UniformRandom(0, 0) : initialDelay;
            TicksPerCall = ticksPerCall;
            _currentTime = 0;
        }
        
        public override int GetTicks(float time)
        {
            int ticks = 0;
            _currentInitialDelay = _currentInitialDelay - time;
            if (_currentInitialDelay < 0)
            {
                _currentTime = _currentTime + time;
                if (_currentTime <= _currentImpulseLength) 
                {
                    ticks = StardustMath.RandomFloor(TicksPerCall * time);
                }
                else if (_currentTime - time <= _currentImpulseLength) 
                {
                    // timestep was too big and it overstepped this impulse. Calculate the ticks for the fraction time
                    ticks = StardustMath.RandomFloor(TicksPerCall * (_currentImpulseLength - _currentTime + time));
                }
                if (_currentTime >= _currentImpulseInterval) 
                {
                    SetCurrentImpulseLength();
                    SetCurrentImpulseInterval();
                    _currentTime = 0;
                }
            }
            return ticks;
        }
        
        /// <summary>
        /// The emitter step after the <code>impulse()</code> call creates a burst of particles.
        /// </summary>
        public void Impulse()
        {
            _currentInitialDelay = -1;
            _currentTime = 0;
        }
        
        private void SetCurrentImpulseLength()
        {
            float len = _impulseLength.Random();
            _currentImpulseLength = len > 0 ? len : 0;
        }
        
        private void SetCurrentImpulseInterval()
        {
            float val = _impulseInterval.Random();
            _currentImpulseInterval = val > 0 ? val : 0;
        }

        private void SetCurrentInitialDelay()
        {
            float val = _initialDelay.Random();
            _currentInitialDelay = val > 0 ? val : 0;
        }
        
        /// <summary>
        /// Resets the clock and randomizes all values
        /// </summary>
        public override void Reset()
        {
            SetCurrentInitialDelay();
            SetCurrentImpulseLength();
            SetCurrentImpulseInterval();
            _currentTime = 0;
        }

        [OnDeserialized]
        public void OnParsingComplete()
        {
            Reset();
        }

    }
}