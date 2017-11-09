using System.Collections.Generic;
using System.Xml.Linq;
using Sparrow.Display;
using Stardust.Emitters;
using Stardust.Handlers;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Sparrow.simple
{
    /// <summary>
    /// Simple, but slow renderer. Doesnt supprt advanced stuff.
    /// </summary>
    public class SimpleSparrowHandler : ParticleHandler
    {

        private DisplayObjectContainer _container;

        public DisplayObjectContainer Container
        {
            set => _container = value;
        }
        
        public override void Reset()
        {
            _container.RemoveAllChildren();
        }

        public override void StepEnd(Emitter emitter, IList<Particle> particles, float time)
        {
            foreach (Particle particle in particles)
            {
                Quad gfx = (Quad) particle.Target;
                gfx.X = particle.X;
                gfx.Y = particle.Y;
                gfx.Scale = particle.Scale;
                gfx.Alpha = particle.Alpha;
            }
        }

        public override void ParticleAdded(Particle particle)
        {
            if (particle.Target == null)
            {
                var quad = new Quad(32, 32, 0xFF00FF);
                quad.PivotX = quad.PivotY = 16;
                particle.Target = quad;
                _container.AddChild(quad);   
            }
            else
            {
                Quad gfx = (Quad) particle.Target;
                _container.AddChild(gfx);
            }
        }

        public override void ParticleRemoved(Particle particle)
        {
            Quad gfx = (Quad) particle.Target;
            gfx.RemoveFromParent();
        }
        
        public override string GetXmlTagName()
        {
            return "SimpleSparrowHandler";
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
        }
    }
}