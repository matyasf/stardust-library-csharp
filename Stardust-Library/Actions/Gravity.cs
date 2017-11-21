using System.Collections.Generic;
using Stardust.Emitters;
using Stardust.Fields;
using Stardust.Geom;
using Stardust.Particles;

namespace Stardust.Actions
{
    public class Gravity : Action
    {

        private List<Field> _fields;

        public Gravity(List<Field> fields)
        {
            Priority = -3;
            if (fields != null)
            {
                _fields = fields;
            }
            else
            {
                _fields = new List<Field>();
                _fields.Add(new UniformField(0, 1));
            }
        }
        
        public List<Field> Fields
        {
            get => _fields;
            set => _fields = value;
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            var len = _fields.Count;
            timeDelta = timeDelta * 100; // acceleration is in m/(s*s)
            for (int i = 0; i < len; i++) {
                var md2D = _fields[i].GetMotionData2D(particle);
                if (md2D != null)
                {
                    particle.Vx += md2D.X * timeDelta;
                    particle.Vy += md2D.Y * timeDelta;
                    Vec2D.RecycleToPool(md2D);
                }
            }
        }
    }
}