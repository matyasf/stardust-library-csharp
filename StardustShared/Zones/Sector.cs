
using System;
using Stardust.Geom;
using Stardust.MathStuff;

namespace Stardust.Zones
{
    /// <summary>
    /// Sector (pie) shaped zone.
    /// </summary>
    public class Sector : Zone
    {
        private static UniformRandom _randomT = new UniformRandom();
        private static Random _rng = new Random();
        private float _minRadius;
        private float _maxRadius;
        private float _minAngle;
        private float _maxAngle;
        private float _minAngleRad;
        private float _maxAngleRad;
        
        public Sector() : this(0, 0, 0, 100, 0, 360) {}
        
        public Sector(float x, float y, float minRadius, float maxRadius, float minAngle, float maxAngle)
        {
            _x = x;
            _y = y;
            MinRadius = minRadius;
            MaxRadius = maxRadius;
            MinAngle = minAngle;
            MaxAngle = maxAngle;
            UpdateArea();
        }

        public float MinRadius
        {
            get => _minRadius;
            set
            {
                _minRadius = value;
                UpdateArea();
            }
        }

        public float MaxRadius
        {
            get => _maxRadius;
            set
            {
                _maxRadius = value;
                UpdateArea();
            }
        }

        public float MinAngle
        {
            get => _minAngle;
            set
            {
                _minAngle = value; 
                UpdateArea();
            }
        }

        public float MaxAngle
        {
            get => _maxAngle;
            set
            {
                _maxAngle = value; 
                UpdateArea();
            }
        }

        public override Vec2D CalculateMotionData2D()
        {
            if (_maxRadius == 0) return Vec2D.GetFromPool(_x, _y);

            _randomT.SetRange(_minAngleRad, _maxAngleRad);
            var theta = _randomT.Random();
            var r = StardustMath.Interpolate(0, _minRadius, 1, _maxRadius, (float)Math.Sqrt(_rng.NextDouble()));

            return Vec2D.GetFromPool(r * (float)Math.Cos(theta), r * (float)Math.Sin(theta));
        }
        
        protected override void UpdateArea()
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
            var dT = _maxAngleRad - _minAngleRad;

            var dRsq = _minRadius * _minRadius - _maxRadius * _maxRadius;

            area = Math.Abs(dRsq * dT);
        }

    }
}