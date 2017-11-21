using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Stardust.Emitters;
using Stardust.Particles;

namespace Stardust.Actions
{
    public class ScaleAnimated : Action
    {
        
        /// <summary>
        /// Number of gradient steps. Higher values result in smoother transition, but more memory usage.
        /// </summary>
        public int NumSteps = 50;

        private float[] _interpolatedValues;
        private uint[] _ratios;
        private float[] _scales;

        [JsonIgnore]
        public uint[] Ratios => _ratios;

        [JsonIgnore]
        public float[] Scales => _scales;
        
        /// <summary>
        /// This is used during serialization
        /// </summary>
        public string ScalesStr {
            set => _scales = Array.ConvertAll(value.Split(','), float.Parse);
            get => String.Join(",", Scales);
        }

        /// <summary>
        /// This is used during serialization
        /// </summary>
        public string RatiosStr
        {
            set => _ratios = Array.ConvertAll(value.Split(','), uint.Parse);
            get => String.Join(",", _ratios);
        }

        public ScaleAnimated(bool setDefaultValues)
        {
            if (setDefaultValues)
            {
                SetGradient(new uint[] {0, 255}, new float[] {1, 2});
            }
        }
        
        /// <summary>
        /// Sets the gradient values. Both vectors must be the same length, and must have less than 16 values.
        /// </summary>
        /// <param name="ratios">Array of uint ratios ordered, in increasing order. First value should be 0, last 255.</param>
        /// <param name="scales">scales that correspond to the ratio</param>
        public void SetGradient(uint[] ratios, float[] scales)
        {
            _ratios = ratios;
            _scales = scales;
            _interpolatedValues = new float[NumSteps];

            var stepSize = NumSteps / 255;
            for (var i = 0; i < _ratios.Length - 1; i++)
            {
                var start = ratios[i] * stepSize;
                var end = ratios[i + 1] * stepSize;
                for (var j = start; j < end; j++)
                {
                    float percent = (float)(j - start) / (end - start);
                    _interpolatedValues[NumSteps - j - 1] = _scales[i] * (1 - percent) + _scales[i + 1] * percent;
                }
            }
        }
        
        
        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            int ratio = (int)((NumSteps - 1) * particle.Life / particle.InitLife);
            particle.Scale = _interpolatedValues[ratio];
        }
        
        [OnDeserialized]
        private void OnSerializationComplete(StreamingContext streamingContext)
        {
            SetGradient(_ratios, _scales);
        }
    }
}