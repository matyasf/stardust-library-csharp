using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using Stardust.Geom;
using Stardust.Particles;
using Stardust.Xml;
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
        
        public IList<Zone> Zones { 
            get => ZoneCollection.Zones;
            set => ZoneCollection.Zones = value;
        }

        public bool InheritVelocity;
        public Vec2D[] Positions;
        private int _prevPos;
        private int _currentPos;
        
        public PositionAnimated() : this(null) {}
        
        public PositionAnimated(IList<Zone> zones)
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
                Vec2D.Pool.Release(vec2D);
            }
        }

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

        #region XML

        public override IEnumerable<StardustElement> GetRelatedObjects()
        {
            return new List<StardustElement>(ZoneCollection.Zones);
        }

        public override string GetXmlTagName()
        {
            return "PositionAnimated";
        }

        public override XElement ToXml()
        {
            XElement xml = base.ToXml();
            ZoneCollection.AddToStardustXml(xml);
            xml.SetAttributeValue("inheritVelocity", InheritVelocity);
            if (Positions != null && Positions.Length > 0)
            {
                throw new NotImplementedException("animated positions are not supported");
            }
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);
            
            ZoneCollection.ParseFromStardustXml(xml, builder);
            InheritVelocity = bool.Parse(xml.Attribute("inheritVelocity").Value);

            if (xml.Attribute("positions") != null)
            {
                Debug.WriteLine("Animated positions cannot be parsed!");
            }
        }

        #endregion
        

        
    }
}