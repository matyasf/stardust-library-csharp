using Stardust.Geom;
using Stardust.Math;
using Stardust.Zones;

namespace Stardust.Initializers
{
    
    /// <summary>
    /// Line segment zone.
    /// </summary>
    public class Line : Contour
    {
        public override float X
        {
            set
            {
                _x = value;
                UpdateArea();
            }
        }
        
        public override float Y
        {
            set
            {
                _y = value;
                UpdateArea();
            }
        }

        private float _x2;

        /// <summary>
        /// The X coordinate of the other end of the line.
        /// </summary>
        public float X2
        {
            get => _x2;
            set
            {
                _x2 = value;
                UpdateArea();
            }
        }
        
        private float _y2;

        /// <summary>
        /// The Y coordinate of the other end of the line.
        /// </summary>
        public float Y2
        {
            get => _y2;
            set
            {
                _y2 = value;
                UpdateArea();
            }
        }

        private RandomBase _random;
        
        public Line() : this(0, 0, 0, 0, null) {}
        
        public Line(float x1, float y1, float x2, float y2, RandomBase random)
        {
            _x = x1;
            _y = y1;
            _x2 = x2;
            _y2 = y2;
            Random = random;
        }

        public override void SetPosition(float xc, float yc)
        {
            var xDiff = _x2 - _x;
            var yDiff = _y2 - _y;
            _x = xc;
            _y = yc;
            _x2 = xc + xDiff;
            _y2 = yc + yDiff;
        }

        public RandomBase Random
        {
            get => _random;
            set
            {
                if (value == null)
                {
                    value = new UniformRandom();
                }
                _random = value;  
            } 
        }
        
        public override Vec2D CalculateMotionData2D()
        {
            throw new System.NotImplementedException();
        }
        
        public override bool Contains(float x, float y)
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdateArea()
        {
            throw new System.NotImplementedException();
        }

        #region XML

        public override string GetXmlTagName()
        {
            return "Line";
        }
        
        #endregion
        
    }
}