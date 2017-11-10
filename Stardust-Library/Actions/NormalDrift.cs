
using System.Collections.Generic;
using System.Xml.Linq;
using Stardust.Emitters;
using Stardust.Geom;
using Stardust.MathStuff;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Actions
{
    /// <summary>
    /// Applies acceleration normal to a particle's velocity to the particle.
    /// </summary>
    public class NormalDrift : Action
    {
        
        /// <summary>
        /// Whether the particles acceleration is divided by their masses before applied to them, true by default.
        /// When set to true, it simulates a gravity that applies equal acceleration on all particles.
        /// </summary>
        public bool Massless;

        private float _timeDeltaOneSec;
        private RandomBase _random;
        private float _max;
        
        public NormalDrift() : this(1, null) {}
        
        public NormalDrift(float max, RandomBase random)
        {
            Massless = true;
            Random = random;
            Max = max;
        }
        
        /// <summary>
        /// The acceleration ranges from -max to max.
        /// </summary>
        public float Max // TODO remove
        {
            get => _max;
            set
            {
                _max = value;
                _random.SetRange(-_max, _max);
            }
        }
        
        /// <summary>
        /// The random object used to generate a random number for the acceleration in the range [-max, max], uniform random by default.
        /// You don't have to set the random object's range. The range is automatically set each time before the random generation.
        /// </summary>
        public RandomBase Random
        {
            get => _random;
            set
            {
                if (value == null)
                {
                    value = new UniformRandom();
                    _random = value;
                    _random.SetRange(-_max, _max);
                }
            }
        }

        public override void PreUpdate(Emitter emitter, float time)
        {
            _timeDeltaOneSec = time * 60;
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            var v = Vec2D.GetFromPool(particle.Vy, particle.Vx);
            v.Length = _random.Random();
            if (!Massless) v.Length /= particle.Mass;
            particle.Vx += v.X * _timeDeltaOneSec;
            particle.Vy += v.Y * _timeDeltaOneSec;
            Vec2D.RecycleToPool(v);
        }

        #region XML

        public override IEnumerable<StardustElement> GetRelatedObjects()
        {
            return new List<StardustElement>() {_random};
        }

        public override string GetXmlTagName()
        {
            return "NormalDrift";
        }

        public override XElement ToXml()
        {
            XElement xml = base.ToXml();
            xml.SetAttributeValue("massless", Massless);
            xml.SetAttributeValue("max", _max);
            xml.SetAttributeValue("random", _random.Name);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);
            Massless = bool.Parse(xml.Attribute("massless").Value);
            Max = float.Parse(xml.Attribute("max").Value);
            Random = (RandomBase)builder.GetElementByName(xml.Attribute("random").Value);
        }

        #endregion
    }
}