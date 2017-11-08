using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Stardust.Geom;
using Stardust.Xml;

namespace Stardust.Zones
{
    public class ZoneCollection
    {
        public IList<Zone> Zones = new List<Zone>();
        
        private static readonly Random Rng = new Random();
        
        public Vec2D GetRandomPointInZones()
        {
            Vec2D md2D = null;
            int numZones = Zones.Count;
            if (numZones > 1)
            {
                float sumArea = 0;
                List<float> areas = new List<float>();
                for (int i = 0; i < numZones; i++)
                {
                    sumArea += Zones[i].GetArea();
                    areas.Add(sumArea);
                }
                float position = (float)Rng.NextDouble() * sumArea;
                for (int i = 0; i < areas.Count; i++)
                {
                    if (position <= areas[i])
                    {
                        md2D = Zones[i].GetPoint();
                        break;
                    }
                }
            }
            else if (numZones == 1)
            {
                md2D = Zones[0].GetPoint();
            }
            return md2D; // returns null if there are no zones
        }

        public bool Contains(float xc, float yc)
        {
            bool contains = false;
            foreach (Zone zone in Zones)
            {
                if (zone.Contains(xc, yc))
                {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        public void AddToStardustXml(XElement stardustXml)
        {
            if (Zones.Count > 0) {
                XElement zones = new XElement("zones");
                stardustXml.Add(zones);
                foreach (Zone zone in Zones) {
                    zones.Add(zone.GetXmlTagName());
                }
            }
        }

        public void ParseFromStardustXml(XElement stardustXml, XmlBuilder builder)
        {
            Zones = new List<Zone>();
            foreach (XElement node in stardustXml.Element("zones").Elements())
            {
                var elem = builder.GetElementByName(node.Attribute("name").Value);
                Zones.Add((Zone)elem);
            }
        }
    }
}