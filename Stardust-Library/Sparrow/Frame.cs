namespace Stardust.Sparrow
{
    public class Frame
    {
        public float ParticleHalfWidth;
        public float ParticleHalfHeight;
        public float TopLeftX;
        public float TopLeftY;
        public float BottomRightX;
        public float BottomRightY;

        public Frame(float topLeftX,
                     float topLeftY,
                     float bottomRightX,
                     float bottomRightY,
                     float halfWidth,
                     float halfHeight)
        {
            TopLeftX = topLeftX;
            TopLeftY = topLeftY;
            BottomRightX = bottomRightX;
            BottomRightY = bottomRightY;
            ParticleHalfWidth = halfWidth;
            ParticleHalfHeight = halfHeight;
        }
    }
}