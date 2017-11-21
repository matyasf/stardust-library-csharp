using System;
using Stardust.MathStuff;

namespace Stardust.Actions.Areas
{
    public class SectorArea : Area
    {
        public float MinRadius;
        public float MaxRadius;
        private float _minAngle;
        private float _maxAngle;
        private float _minAngleRad;
        private float _maxAngleRad;
        
        public SectorArea() : this(0, 0, 0, 100, 0, 360) {}
        
        public SectorArea(float x, float y, float minRadius, float maxRadius,
                          float minAngle, float maxAngle)
        {
            X = x;
            Y = y;
            MinRadius = minRadius;
            MaxRadius = maxRadius;
            MinAngle = minAngle;
            MaxAngle = maxAngle;
        }
        
        /// <summary>
        /// The minimum angle of the sector
        /// </summary>
        public float MinAngle
        {
            get { return _minAngle; }
            set
            {
                _minAngle = value;
                UpdateRadValues();
            }
        }
        
        /// <summary>
        /// The maximum angle of the sector.
        /// </summary>
        public float MaxAngle
        {
            get { return _maxAngle; }
            set
            {
                _maxAngle = value;
                UpdateRadValues();
            }
        }

        private void UpdateRadValues()
        {
            _minAngleRad = _minAngle * StardustMath.DegreeToRadian;
            _maxAngleRad = _maxAngle * StardustMath.DegreeToRadian;
            if (Math.Abs(_minAngleRad) > StardustMath.TwoPi)
            {
                _minAngleRad = _minAngleRad % StardustMath.TwoPi;
            }
            if (Math.Abs(_maxAngleRad) > StardustMath.TwoPi)
            {
                _maxAngleRad = _maxAngleRad % StardustMath.TwoPi;
            }
        }

        public override bool Contains(float x, float y)
        {
            var dx = X - x;
            var dy = Y - y;
            var squaredDistance = dx * dx + dy * dy;
            bool isInsideOuterCircle = (squaredDistance <= MaxRadius * MaxRadius);
            if (!isInsideOuterCircle) {
                return false;
            }
            bool isInsideInnerCircle = (squaredDistance <= MinRadius * MinRadius);
            if (isInsideInnerCircle) {
                return false;
            }
            float angle = (float)Math.Atan2(dy, dx) + StardustMath.Pi;
            // TODO: does not work for edge cases, e.g. when minAngle = -20 and maxAngle = 20
            if (angle > _maxAngleRad || angle < _minAngleRad) {
                return false;
            }
            return true;
        }
    }
}