using System;

namespace Stardust.MathStuff
{
    /// <summary>
    /// This class provides common mathematical constants and methods.
    /// </summary>
    public class StardustMath
    {
        public const float TwoPi = (float)(2 * System.Math.PI);
        public const float Pi = (float)System.Math.PI;
        public const float DegreeToRadian = (float) (System.Math.PI / 180);
        public const float RadianToDegree = (float) (180 / System.Math.PI);
        private static readonly Random RandomGen = new Random();
        
        /// <summary>
        /// Returns 0 or 1 with a probability based on the non-integer digits.
        /// </summary>
        public static int RandomFloor(float num)
        {
            int floor = (int)System.Math.Floor(num);
            return floor + (((num - floor) > RandomGen.NextDouble()) ? 1 : 0);
        }
        
        /// <summary>
        /// Interpolates linearly between two values.
        /// </summary>
        public static float Interpolate(float x1, float y1, float x2, float y2, float x3)
        {
            return y1 - ((y1 - y2) * (x1 - x3) / (x1 - x2));
        }
    }
}