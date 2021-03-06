﻿using System;
using System.Collections.Generic;
using Stardust.Geom;

namespace Stardust.Zones
{
    public class ZoneCollection
    {
        public List<Zone> Zones = new List<Zone>();
        
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
        
    }
}