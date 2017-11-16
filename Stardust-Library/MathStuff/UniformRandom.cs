namespace Stardust.MathStuff
{
    /// <summary>
    /// This class generates uniformly distributed random numbers.
    /// </summary>
    public class UniformRandom : RandomBase
    {
        /// <summary>
        /// The expected value of the random number.
        /// </summary>
        public float Center;

        /// <summary>
        /// The variation of the random number.
        ///
        /// <para>
        /// The range of the generated random number is [center - radius, center + radius].
        /// </para>
        /// </summary>
        public float Radius;

        public UniformRandom() : this(0.5f, 0) {}
        
        public UniformRandom(float center, float radius)
        {
            Center = center;
            Radius = radius;
        }
        
        public override float Random()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (Radius != 0f)
            {
                return (float)(Radius * 2 * (RandomGen.NextDouble() - 0.5) + Center);
            }
            return Center;
        }
        
        public override void SetRange(float lowerBound, float upperBound)
        {
            float diameter = upperBound - lowerBound;
            Radius = 0.5f * diameter;
            Center = lowerBound + Radius;
        }

        public override float[] GetRange()
        {
            return new[]{Center - Radius, Center + Radius};
        }
        
    }
}