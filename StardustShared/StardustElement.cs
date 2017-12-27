
using System.Collections.Generic;

namespace Stardust
{
    /// <summary>
    /// All Stardust elements are subclasses of this class.
    /// </summary>
    public abstract class StardustElement
    {
        private static readonly Dictionary<string, int> ElementCounter = new Dictionary<string, int>();
        
        public string Name;

        public StardustElement()
        {
            string str = GetType().Name;
            if (!ElementCounter.ContainsKey(str)) {
                ElementCounter[str] = 0;
            }
            else {
                ElementCounter[str]++;
            }
            Name = str + "_" + ElementCounter[str];
        }
    
    }
}