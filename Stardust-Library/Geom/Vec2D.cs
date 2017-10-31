using Stardust.Math;

namespace Stardust.Geom
{
    using System;
    
    public class Vec2D : IDisposable
    {
        public static readonly Pool<Vec2D> Pool = new Pool<Vec2D>(100, pool => new Vec2D());
        
        public float X;
        public float Y;
        
        private Vec2D(float x = 0, float y = 0) {
            X = x;
            Y = y;
        }

        public float Length
        {
            get { return (float)Math.Sqrt(X * X + Y * Y); }
            set
            {
                if (X == 0 && Y == 0) return;
                float factor = value / Length;
                X = X * factor;
                Y = Y * factor;
            }
        }

        public void SetTo(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float Dot(Vec2D vector)
        {
            return (X * vector.X) + (Y * vector.Y);
        }

        public Vec2D Project(Vec2D target)
        {
            Vec2D temp = Clone();
            temp.ProjectThis(target);
            return temp;
        }
        
        public void ProjectThis(Vec2D target)
        {
            Vec2D temp = Pool.Acquire();
            temp.X = target.X;
            temp.Y = target.Y;
            temp.Length = 1;
            temp.Length = Dot(temp);
            X = temp.X;
            Y = temp.Y;
            Pool.Release(temp);
        }
        
        public Vec2D Rotate(float angle, bool useRadian)
        {
            if (!useRadian) angle = angle * StardustMath.DegreeToRadian;
            var originalX = X;
            X = (float)(originalX * Math.Cos(angle) - Y * Math.Sin(angle));
            Y = (float)(originalX * Math.Sin(angle) + Y * Math.Cos(angle));
            return this;
        }

        /// <summary>
        /// The angle between the vector and the positive x axis in degrees.
        /// </summary>
        public float Angle
        {
            get { return (float)Math.Atan2(Y, X) * StardustMath.RadianToDegree; }
            set
            {
                float originalLength = Length;
                float rad = value * StardustMath.DegreeToRadian;
                X = (float)(originalLength * Math.Cos(rad));
                Y = (float)(originalLength * Math.Sin(rad));
            }
        }

        public Vec2D Clone()
        {
            Vec2D clone = Pool.Acquire();
            clone.X = X;
            clone.Y = Y;
            return clone;
        }

        public void Dispose()
        {
            X = 0;
            Y = 0;
            Pool.Release(this);
        }
    }
}