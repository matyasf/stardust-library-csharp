using System;

namespace Stardust.Math
{
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
    }
}