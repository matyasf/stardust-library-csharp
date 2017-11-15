
using System.Xml.Linq;
using System.Xml.Serialization;
using Stardust.MathStuff;
using Stardust.Xml;

namespace Stardust.Clocks
{
    /// <summary>
    /// Causes the emitter to create particles at a steady rate.
    /// </summary>
    public class SteadyClock : Clock
    {

        /// <summary>
        /// How many particles to create in each second.
        ///
        /// If less than one, it's the probability of an emitter to create a single particle in each second.
        /// </summary>
        public float TicksPerCall;

        private RandomBase _initialDelay;
        private float currentInitialDelay;
        private float currentTime;

        public SteadyClock() : this(1, null) {}
        
        public SteadyClock(float ticksPerCall = 1, RandomBase initialDelay = null)
        {
            TicksPerCall = ticksPerCall;
            InitialDelay = _initialDelay == null ? new UniformRandom(0, 0) : _initialDelay;
            currentTime = 0;
        }
        
        public RandomBase InitialDelay
        {
            get { return _initialDelay; }
            set
            {
                _initialDelay = value;
                SetCurrentInitialDelay();
            }
        }
        
        public override int GetTicks(float time)
        {
            currentTime += time;
            if (currentTime > currentInitialDelay)
            {
                return StardustMath.RandomFloor(TicksPerCall * time);
            }
            return 0;
        }

        /// <inheritdoc cref="Clock.Reset"/>
        public override void Reset()
        {
            currentTime = 0;
            SetCurrentInitialDelay();
        }

        private void SetCurrentInitialDelay()
        {
            float val = _initialDelay.Random();
            currentInitialDelay = val > 0 ? val : 0;
        }
        
        #region XML
        
        public override string GetXmlTagName()
        {
            return "SteadyClock";
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            TicksPerCall = float.Parse(xml.Attribute("ticksPerCall").Value);
            _initialDelay = builder.GetElementByName(xml.Attribute("initialDelay").Value) as RandomBase;
        }
        
        #endregion
    }
}