using System.Collections.Generic;

namespace Stardust.Actions.Areas
{
    public class AreaCollection
    {
        public List<Area> Areas = new List<Area>();

        public bool Contains(float xc, float yc)
        {
            foreach (var area in Areas)
            {
                if (area.Contains(xc, yc))
                {
                    return true;
                }
            }
            return false;
        }
    }
}