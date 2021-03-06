﻿using Stardust.Geom;
using Stardust.MathStuff;

namespace Stardust.Zones
{
    /// <summary>
    /// Rectangular zone
    /// </summary>
    public class RectZone : Zone
    {

        private RandomBase _randomX;
        private RandomBase _randomY;
        private float _width;
        private float _height;
        
        public RectZone() : this(0, 0, 150, 50, null, null) {}
        
        public RectZone(float x, float y, float width, float height, RandomBase randomX, RandomBase randomY)
        {
            if (randomX == null) randomX = new UniformRandom();
            if (randomY == null) randomY = new UniformRandom();

            _x = x;
            _y = y;
            RandomX = randomX;
            RandomY = randomY;
            Width = width;
            Height = height;
        }
        
        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                _randomX.SetRange(0, value);
                UpdateArea();
            }
        }
        
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                _randomY.SetRange(0, value);
                UpdateArea();
            }
        }
        
        public RandomBase RandomX
        {
            get => _randomX;
            set
            {
                if (value == null) value = new UniformRandom();
                _randomX = value;
                _randomX.SetRange(0, _width);
            }
        }
        
        public RandomBase RandomY
        {
            get => _randomY;
            set
            {
                if (value == null) value = new UniformRandom();
                _randomY = value;
                _randomY.SetRange(0, _height);
            }
        }
        
        protected override void UpdateArea()
        {
            area = _width * _height;
        }

        public override Vec2D CalculateMotionData2D()
        {
            return Vec2D.GetFromPool(_randomX.Random(), _randomY.Random());
        }

    }
}