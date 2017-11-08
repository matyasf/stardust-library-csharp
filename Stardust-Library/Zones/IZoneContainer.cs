
using System.Collections.Generic;

namespace Stardust.Zones
{
    public interface IZoneContainer
    {
        IList<Zone> Zones { get; set; }
        
    }
}