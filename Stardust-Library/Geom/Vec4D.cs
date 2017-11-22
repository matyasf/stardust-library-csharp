using System;

namespace Stardust.Geom
{
    public class Vec4D : IDisposable
    {
        private static readonly Pool<Vec4D> Pool = new Pool<Vec4D>(pool => new Vec4D());

        public static Vec4D GetFromPool(float x = 0f, float y = 0f, float vx = 0f, float vy = 0f)
        {
            Vec4D vec = Pool.Acquire();
            vec.SetTo(x, y, vx, vy);
            return vec;
        }

        public static void RecycleToPool(Vec4D vec)
        {
            Pool.Release(vec);
        }
    
        public float X;
        public float Y;
        public float Vx;
        public float Vy;
        
        
        public Vec4D() {}
        
        public Vec4D(float x, float y, float vx, float vy)
        {
            SetTo(x, y, vx, vy);
        }

        public void SetTo(float x, float y, float vx, float vy)
        {
            X = x;
            Y = y;
            Vx = vx;
            Vy = vy;
        }

        public void Dispose()
        {
            X = 0;
            Y = 0;
            Vx = 0;
            Vy = 0;
            Pool.Release(this);
        }
    }
}