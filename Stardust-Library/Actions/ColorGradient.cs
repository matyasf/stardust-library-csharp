using System;
using System.Xml.Linq;
using System.Xml.Serialization;
using Stardust.Emitters;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Actions
{
    /// <summary>
    /// Alters a particle's color during its lifetime based on a gradient.
    /// </summary>
    public class ColorGradient : Action
    {

        /// <summary>
        /// Number of gradient steps. Higher values result in smoother transition, but more memory usage.
        /// </summary>
        public int NumSteps = 200;
        
        [XmlAttribute]
        public uint[] Colors;
        [XmlAttribute]
        public float[] Ratios;
        [XmlAttribute]
        public float[] Alphas;
        
        private float[] _colorRs; // 0..1
        private float[] _colorGs;
        private float[] _colorBs;
        private float[] _colorAlphas;
        
        public ColorGradient() : this(false) {}
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="setDefaultValues">Set some default values to start with. 
        ///     Leave it false if you set value manually to prevent parsing twice</param>
        public ColorGradient(bool setDefaultValues)
        {
            if (setDefaultValues)
            {
                SetGradient(new uint[] {0x00FF00, 0xFF0000}, new float[] {0, 255}, new float[]{1, 1});
            }
        }
        
        /// <summary>
        /// Sets the gradient values. All arrays must be the same length.
        /// </summary>
        /// <param name="colors">Array of colors in HEX RGB</param>
        /// <param name="ratios">Array of ratios ordered, in increasing order. First value must be 0, last 255.</param>
        /// <param name="alphas">Array of alphas in the 0-1 range.</param>
        public void SetGradient(uint[] colors, float[] ratios, float[] alphas)
        {
            Colors = colors;
            Ratios = ratios;
            Alphas = alphas;
            _colorRs = new float[NumSteps];
            _colorGs = new float[NumSteps];
            _colorBs = new float[NumSteps];
            _colorAlphas = new float[NumSteps];
            // create gradient values via interpolation
            float stepSize = NumSteps / 255f;
            for (int i = 0; i < Colors.Length - 1; i++)
            {
                // see https://codepen.io/Tobsta/post/programmatically-making-gradients 
                float colorRStart = ((Colors[i] >> 16) & 0xFF); // 0..255
                float colorREnd = ((Colors[i+1] >> 16) & 0xFF);

                float colorGStart = ((Colors[i] >> 8) & 0xFF); // 0..255
                float colorGEnd = ((Colors[i + 1] >> 8) & 0xFF);

                float colorBStart = (Colors[i] & 0xFF); // 0..255
                float colorBEnd = (Colors[i + 1] & 0xFF);

                float start = ratios[i] * stepSize;
                float end = ratios[i + 1] * stepSize;
                CalcGradient(start, end, colorRStart, colorREnd, _colorRs);
                CalcGradient(start, end, colorGStart, colorGEnd, _colorGs);
                CalcGradient(start, end, colorBStart, colorBEnd, _colorBs);
                CalcAlpha(start, end, Alphas[i], alphas[i + 1], _colorAlphas);
            }
            for (int i = 0; i < _colorRs.Length; i++)
            {
                _colorRs[i] = _colorRs[i] / 255f;
                _colorGs[i] = _colorGs[i] / 255f;
                _colorBs[i] = _colorBs[i] / 255f;
            }
        }

        private void CalcGradient(float start, float end, float colorStart, float colorEnd, float[] array)
        {
            colorStart = colorStart * colorStart;
            colorEnd = colorEnd * colorEnd;
            for (float j = start; j < end; j++)
            {
                float percent = (j - start) / (end - start);
                array[(int)j] = (float)Math.Floor(Math.Sqrt(colorStart * (1f - percent) + colorEnd * percent));
            }
        }

        private void CalcAlpha(float start, float end, float alphaStart, float alphaEnd, float[] array)
        {
            for (float j = start; j < end; j++)
            {
                float percent = (j - start) / (end - start);
                array[(int)j] = alphaStart * (1f - percent) + alphaEnd * percent;
            }
        }

        public override void Update(Emitter emitter, Particle particle, float timeDelta, float currentTime)
        {
            int ratio = (int)((NumSteps - 1) * (1 - particle.Life / particle.InitLife));
            particle.ColorR = _colorRs[ratio];
            particle.ColorB = _colorBs[ratio];
            particle.ColorG = _colorGs[ratio];
            particle.Alpha = _colorAlphas[ratio];
        }

        #region XML

        public override string GetXmlTagName()
        {
            return "ColorGradient";
        }

        public override XElement ToXml()
        {
            XElement xml = base.ToXml();

            string colorsStr = string.Join(",", _colorRs);
            string ratiosStr = string.Join(",", Ratios);
            string alphasStr = string.Join(",", Alphas);
            
            xml.SetAttributeValue("colors", colorsStr);
            xml.SetAttributeValue("ratios", ratiosStr);
            xml.SetAttributeValue("alphas", alphasStr);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            base.ParseXml(xml, builder);
            
            string[] colors = xml.Attribute("colors").Value.Split(',');
            string[] ratios = xml.Attribute("ratios").Value.Split(',');
            string[] alphas = xml.Attribute("alphas").Value.Split(',');
            
            SetGradient(Array.ConvertAll(colors, uint.Parse), 
                Array.ConvertAll(ratios, float.Parse), 
                Array.ConvertAll(alphas, float.Parse));
        }

        #endregion
        
    }
}