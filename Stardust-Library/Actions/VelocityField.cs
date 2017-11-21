using System.Collections.Generic;
using Stardust.Emitters;
using Stardust.Fields;
using Stardust.Geom;
using Stardust.Particles;

namespace Stardust.Actions
{
    /// <summary>
    /// Alters a particle's velocity based on a vector field.
    ///
    /// <para>
    /// The returned value of a field is a <code>Vec2D</code> object, which is a 2D value class.
    /// The particle's velocity X and Y components are set to the <code>Vec2D</code> object's <code>x</code>
    ///  and <code>y</code> properties, respectively.
    /// </para>
    ///
    /// </summary>
    public class VelocityField : Action
    {

        private Field _field;

        public VelocityField(Field field)
        {
            _priority = -2;
            if (field != null)
            {
                _field = field;
            }
            else
            {
                _field = new UniformField(100, 100);
            }
        }

        public List<Field> Fields
        {
            get
            {
                var ret = new List<Field>();
                if (_field != null)
                {
                    ret.Add(_field);
                }
                return ret;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    _field = value[0];
                }
            }
        }
        
        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            if (_field == null)
            {
                return;
            }
            var md2D = _field.GetMotionData2D(particle);
            particle.Vx = md2D.X;
            particle.Vy = md2D.Y;
            Vec2D.RecycleToPool(md2D);
        }
    }
}