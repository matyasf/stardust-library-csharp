using System;
using System.Collections.Generic;
using Stardust.Actions.Areas;
using Stardust.Emitters;
using Stardust.Geom;
using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Actions
{
    /// <summary>
    /// Causes particles to change acceleration specified area.
    ///
    /// <para>
    /// Default priority = -6;
    /// </para>
    /// </summary>
    public class AccelerationArea : Action, IAreaContainer
    {
        protected readonly AreaCollection AreaCollection;
        
        /// <summary>
        /// Inverts the area region.
        /// </summary>
        public bool Inverted;

        /// <summary>
        /// The acceleration applied in each step to particles inside the area.
        /// </summary>
        public float Acceleration;
        
        /// <summary>
        /// Flag whether to use the particle's speed or the direction variable. Default is true.
        /// </summary>
        public bool UseParticleDirection;
        
        /// <summary>
        /// Acceleration type: 0 = constant, 1 = linear
        /// </summary>
        public int AccelerationType = 1;
        
        private float _directionX;
        
        private float _directionY;
        
        // Normalized linear acceleration, so values are in the same range as constant.
        private float _accelNormalized;
        
        /// <summary>
        /// The direction of the acceleration. Only used if useParticleDirection is false
        /// </summary>
        public float Direction
        {
            get => (float)Math.Atan2(_directionY, _directionX) * StardustMath.RadianToDegree;
            set
            {
                float rad = value * StardustMath.DegreeToRadian;
                _directionX = (float)Math.Cos(rad);
                _directionY = (float)Math.Sin(rad);
            }
        }
        
        public List<Area> Areas
        {
            get => AreaCollection.Areas;
            set => AreaCollection.Areas = value;
        }
        
        public AccelerationArea() : this(null, false) {}
        
        public AccelerationArea(List<Area> areas, bool inverted)
        {
            _priority = -6;
            Inverted = inverted;
            Acceleration = 3;
            UseParticleDirection = true;
            Direction = -90;
            AreaCollection = new AreaCollection();
            if (areas != null)
            {
                AreaCollection.Areas = areas;
            }
            else
            {
                AreaCollection.Areas.Add(new RectArea());
            }
        }
        
        public override void PreUpdate(Emitter emitter, float time)
        {
            _accelNormalized = Acceleration / 20 + 1;
        }
        
        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            bool affected  = AreaCollection.Contains(particle.X, particle.Y);
            if (Inverted)
            {
                affected = !affected;
            }
            if (affected)
            {
                if (UseParticleDirection)
                {
                    if (AccelerationType == 0)
                    {
                        Vec2D v = Vec2D.GetFromPool(particle.Vx, particle.Vy);
                        float vecLength = v.Length;
                        if (vecLength > 0)
                        {
                            float finalVal = vecLength + Acceleration;
                            if (finalVal < 0)
                            {
                                finalVal = 0;
                            }
                            v.Length = finalVal;
                            particle.Vx = v.X;
                            particle.Vy = v.Y;
                        }
                        Vec2D.RecycleToPool(v);
                    }
                    else if (AccelerationType == 1)
                    {
                        particle.Vx *= _accelNormalized;
                        particle.Vy *= _accelNormalized;
                    }
                }
                else
                {
                    if (AccelerationType == 0)
                    {
                        particle.Vx = particle.Vx + Acceleration * _directionX;
                        particle.Vy = particle.Vy + Acceleration * _directionY;
                    }
                    else if (AccelerationType == 1)
                    {
                        particle.Vx *= Acceleration * _directionX;
                        particle.Vy *= Acceleration * _directionY;
                    }
                }
            }
        }

    }
}