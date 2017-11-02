using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Stardust.Geom;
using Stardust.Particles;
using Stardust.Xml;
using Stardust.Zones;

namespace Stardust.Initializers
{
    public class Velocity : Initializer
    {

        protected ZoneCollection ZoneCollection;

        public IList<Zone> Zones
        {
            get => ZoneCollection.Zones;
            set => ZoneCollection.Zones = value;
        }
        
        public Velocity() : this(null) {}

        public Velocity(IList<Zone> zones)
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

        #region XML

        public override IEnumerable<StardustElement> GetRelatedObjects()
        {
            // is this OK?
            return new List<StardustElement>(ZoneCollection.Zones);
        }

        public override string GetXmlTagName()
        {
            return "Velocity";
        }

        public override XElement ToXml()
        {
            var xml = base.ToXml();
            ZoneCollection.AddToStardustXml(xml);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);
            ZoneCollection.ParseFromStardustXml(xml, builder);
        }

        #endregion
        
    }
}