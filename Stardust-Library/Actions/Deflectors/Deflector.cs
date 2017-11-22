using Stardust.Geom;
using Stardust.Particles;

namespace Stardust.Actions.Deflectors
{
    /// <summary>
    /// Used along with the <code>Deflect</code> action.
    /// </summary>
    public abstract class Deflector : StardustElement
    {
        public bool Active;
        public float Bounce;
        /// <summary>
        /// Determines how slippery the surfaces are. A value of 1 (default) means that the surface is fully slippery,
        /// a value of 0 means that particles will not slide on its surface at all.
        /// </summary>
        public float Slipperiness;

        public Deflector()
        {
            Active = true;
            Bounce = 0.8f;
            Slipperiness = 1;
        }

        public Vec4D GetMotionData4D(Particle particle)
        {
            if (Active)
            {
                return CalculateMotionData4D(particle);
            }
            return null;
        }

        protected abstract Vec4D CalculateMotionData4D(Particle particle);
        
        
    }
}