using System;
using Stardust.Geom;
using Stardust.MathStuff;

namespace Stardust.Actions.Areas
{
    public abstract class Area : StardustElement
    {
        
        public float X;
        public float Y;
        
        protected float _rotation;
        protected float angleCos;
        protected float angleSin;
        
        public Area()
        {
            Rotation = 0;
        }
        
        public abstract bool Contains(float x, float y);
        
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                var valInRad = value * StardustMath.DegreeToRadian;
                angleCos = (float)Math.Cos(valInRad);
                angleSin = (float)Math.Sin(valInRad);
                _rotation = value;
            }
        }
        
        public void SetPosition(float xc, float yc)
        {
            X = xc;
            Y = yc;
        }
        
        public Vec2D GetPosition()
        {
            return Vec2D.GetFromPool(X, Y);
        }
    
    }
}