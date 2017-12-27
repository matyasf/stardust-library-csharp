namespace Stardust.Actions.Areas
{
    public class CircleArea : Area
    {
        private float _radius;
        private float _radiusSQ;

        public CircleArea(float x, float y, float radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                _radiusSQ = value * value;
            }
        }

        public override bool Contains(float x, float y)
        {
            float dx = X - x;
            float dy = Y - y;
            return (dx * dx + dy * dy <= _radiusSQ) ? true : false;
        }
    }
}