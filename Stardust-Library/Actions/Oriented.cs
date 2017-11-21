using System;
using Stardust.Emitters;
using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Actions
{
    /// <summary>
    /// Causes particles' rotation to align to their velocities.
    /// </summary>
    public class Oriented : Action
    {
        /// <summary>
        /// How fast the particles align to their velocities, 0 means no alignment at all.
        /// </summary>
        public float Factor;

        /// <summary>
        /// The rotation angle offset in degrees.
        /// </summary>
        public float Offset;

        private float _timeDeltaOneSec;
        private float _f;
        private float _os;

        public Oriented(float factor, float offset)
        {
            _priority = -6;

            Factor = factor;
            Offset = offset;
        }

        public override void PreUpdate(Emitter emitter, float time)
        {
            _f = (float)Math.Pow(Factor, 0.1 / time);
            _os = Offset + 90;
            _timeDeltaOneSec = (time + Emitter.TimeStepCorrectionOffset) * 60;
            if (_timeDeltaOneSec > 1)
            {
                _timeDeltaOneSec = 1; // to prevent overalignment
            }
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            float displacement = (float)(Math.Atan2(particle.Vy, particle.Vx) * StardustMath.RadianToDegree + _os) - particle.Rotation;
            particle.Rotation += _f * displacement * _timeDeltaOneSec;
        }
    }
}