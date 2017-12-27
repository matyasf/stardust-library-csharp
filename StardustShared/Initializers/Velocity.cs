
using System.Collections.Generic;
using Stardust.Geom;
using Stardust.Particles;
using Stardust.Zones;

namespace Stardust.Initializers
{
    public class Velocity : Initializer
    {

        protected readonly ZoneCollection ZoneCollection;

        public List<Zone> Zones
        {
            get => ZoneCollection.Zones;
            set => ZoneCollection.Zones = value;
        }
        
        public Velocity() : this(null) {}

        public Velocity(List<Zone> zones)
        {
            ZoneCollection = new ZoneCollection();
            if (zones != null)
            {
                ZoneCollection.Zones = zones;
            }
            else
            {
                ZoneCollection.Zones.Add(new SinglePoint(0, 0));
            }
        }

        public override void Initialize(Particle particle)
        {
            Vec2D vec2D = ZoneCollection.GetRandomPointInZones();
            if (vec2D != null)
            {
                particle.Vx += vec2D.X;
                particle.Vy += vec2D.Y;
                vec2D.Dispose();
            }
        }
        
    }
}