using System.Collections.Generic;
using Newtonsoft.Json;
using Stardust.Geom;
using Stardust.Particles;
using Stardust.Zones;

namespace Stardust.Initializers
{
    
    /// <summary>
    /// Sets a particle's initial position based on the zone plus on a value in the positions array.
    /// The current position is: positions[currentFrame] + random point in the zone.
    /// </summary>
    public class PositionAnimated : Initializer, IZoneContainer
    {

        protected ZoneCollection ZoneCollection;
        
        public List<Zone> Zones { 
            get => ZoneCollection.Zones;
            set => ZoneCollection.Zones = value;
        }
        
        public bool InheritVelocity;
        
        [JsonIgnore] // TODO parse this
        public Vec2D[] Positions;
        private int _prevPos;
        private int _currentPos;
        
        public PositionAnimated() : this(null) {}
        
        public PositionAnimated(List<Zone> zones)
        {
            ZoneCollection = new ZoneCollection();
            if (zones != null) 
            {
                ZoneCollection.Zones = zones;
            }
            else {
                ZoneCollection.Zones.Add(new RectZone());
            }
        }

        public override void DoInitialize(IEnumerable<Particle> particles, float currentTime)
        {
            if (Positions != null)
            {
                _currentPos = (int)(currentTime % Positions.Length);
                _prevPos = (_currentPos > 0) ? _currentPos - 1 : Positions.Length - 1;
            }
            base.DoInitialize(particles, currentTime);
        }

        public override void Initialize(Particle particle)
        {
            Vec2D vec2D = ZoneCollection.GetRandomPointInZones();
            if (vec2D != null) 
            {
                particle.X = vec2D.X;
                particle.Y = vec2D.Y;

                if (Positions != null)
                {
                    particle.X = vec2D.X + Positions[_currentPos].X;
                    particle.Y = vec2D.Y + Positions[_currentPos].Y;

                    if (InheritVelocity) 
                    {
                        particle.Vx += Positions[_currentPos].X - Positions[_prevPos].X;
                        particle.Vy += Positions[_currentPos].Y - Positions[_prevPos].Y;
                    }
                }
                else {
                    particle.X = vec2D.X;
                    particle.Y = vec2D.Y;
                }
                Vec2D.RecycleToPool(vec2D);
            }
        }

        [JsonIgnore]
        public Vec2D CurrentPosition
        {
            get
            {
                if (Positions != null)
                {
                    return Positions[_currentPos];
                }
                return null;
            }
        }
        
    }
}