using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Sparrow.Display;
using Sparrow.Textures;
using Stardust.Emitters;
using Stardust.Particles;
using Stardust.Xml;

namespace Stardust.Handlers.Sparrow
{
    public class SparrowHandler : ParticleHandler
    {
        private static readonly Random Rng = new Random();
        private uint _blendMode; // TODO use stronly typed value in Sparrow-sharp
        private int _spriteSheetAnimationSpeed;
        private TextureSmoothing _smoothing;
        private bool _isSpriteSheet;
        private bool _premultiplyAlpha = true;
        private bool _spriteSheetStartAtRandomFrame;
        private int _totalFrames;
        private IList<SubTexture> _textures;
        private SparrowRenderer _renderer;
        private float _timeSinceLastStep;

        public SparrowHandler()
        {
            _timeSinceLastStep = 0;
            _spriteSheetAnimationSpeed = 1;
        }
        
        public override void Reset()
        {
            _timeSinceLastStep = 0;
            _renderer.AdvanceTime(new List<Particle>());
        }

        public DisplayObjectContainer Container
        {
            set
            {
                CreateRendererIfNeeded();
                value.AddChild(_renderer);
            }
        }

        private void CreateRendererIfNeeded()
        {
            if (_renderer == null)
            {
                _renderer = new SparrowRenderer();
                _renderer.BlendMode = _blendMode;
                _renderer.TexSmoothing = _smoothing;
                _renderer.PremultiplyAlpha = _premultiplyAlpha;
            }
        }

        public override void StepEnd(Emitter emitter, IList<Particle> particles, float time)
        {
            if (_isSpriteSheet && _spriteSheetAnimationSpeed > 0) {
                _timeSinceLastStep = _timeSinceLastStep + time;
                if (_timeSinceLastStep > 1f/_spriteSheetAnimationSpeed)
                {
                    int stepSize = (int)System.Math.Floor(_timeSinceLastStep * _spriteSheetAnimationSpeed);
                    var mNumParticles = particles.Count;
                    for (int i = 0; i < mNumParticles; ++i) {
                        Particle particle = particles[i];
                        int currFrame = particle.CurrentAnimationFrame;
                        currFrame = currFrame + stepSize;
                        if (currFrame >= _totalFrames) {
                            currFrame = 0;
                        }
                        particle.CurrentAnimationFrame = currFrame;
                    }
                    _timeSinceLastStep = 0;
                }
            }
            _renderer.AdvanceTime(particles);
        }

        public override void ParticleAdded(Particle particle)
        {
            if (_isSpriteSheet) {
                int currFrame = 0;
                if (_spriteSheetStartAtRandomFrame) {
                    currFrame = (int)(Rng.NextDouble() * _totalFrames);
                }
                particle.CurrentAnimationFrame = currFrame;
            }
            else {
                particle.CurrentAnimationFrame = 0;
            }
        }
        
        public override void ParticleRemoved(Particle particle)
        {   
        }

        public SparrowRenderer Renderer => _renderer;
        
        public int SpriteSheetAnimationSpeed
        {
            get => _spriteSheetAnimationSpeed;
            set
            {
                _spriteSheetAnimationSpeed = value;
                if (_textures != null)
                {
                    Textures = _textures;
                }
            }
        }
        
        public bool SpriteSheetStartAtRandomFrame
        {
            get => _spriteSheetStartAtRandomFrame;
            set => _spriteSheetStartAtRandomFrame = value;
        }
        
        public bool IsSpriteSheet => _isSpriteSheet;
        
        public bool Smoothing // TODO make this an enum?
        {
            get => _smoothing != TextureSmoothing.None;
            set
            {
                if (value == true) {
                    _smoothing = TextureSmoothing.Bilinear;
                }
                else {
                    _smoothing = TextureSmoothing.None;
                }
                CreateRendererIfNeeded();
                _renderer.TexSmoothing = _smoothing;
            }
        }

        public bool PremultiplyAlpha
        {
            get => _premultiplyAlpha;
            set
            {
                _premultiplyAlpha = value;
                CreateRendererIfNeeded();
                _renderer.PremultiplyAlpha = value;
            }
        }
        public uint BlendMode
        {
            get => _blendMode;
            set
            {
                _blendMode = value;
                CreateRendererIfNeeded();
                _renderer.BlendMode = value;
            }
        }
        
        /// <summary>
        /// Sets the textures directly. Stardust can batch the simulations resulting multiple simulations using
        ///  just one draw call. To have this working the following must be met:
        ///  - The textures must come from the same sprite sheet. (= they must have the same base texture)
        ///  - The simulations must have the same render target, tinted, smoothing, blendMode, same filter
        ///    and the same premultiplyAlpha values.
        /// </summary>
        /// <exception cref="ArgumentException">The value is not or has 0 elements</exception>
        /// <exception cref="Exception">Textures do not share the same root.</exception>
        public IList<SubTexture> Textures
        {
            get => _textures;
            set
            {
                if (value == null || value.Count == 0) {
                    throw new ArgumentException("the textures parameter cannot be null and needs to hold at least 1 element");
                }
                CreateRendererIfNeeded();
                _isSpriteSheet = value.Count > 1;
                _textures = value;
                List<Frame> frames = new List<Frame>();
                foreach (SubTexture texture in value) {
                    if (texture.Root != value[0].Root) {
                        throw new Exception("The texture " + texture + " does not share the same base root with others");
                    }
                    // TODO use the transformationMatrix
                    Frame frame = new Frame(
                        texture.Region.X / texture.Root.Width,
                        texture.Region.Y / texture.Root.Height,
                        (texture.Region.X + texture.Region.Width) / texture.Root.Width,
                        (texture.Region.Y + texture.Region.Height) / texture.Root.Height,
                        texture.Width * 0.5f,
                        texture.Height * 0.5f);
                    frames.Add(frame);
                }
                _totalFrames = frames.Count;
                _renderer.SetTextures(value[0].Root, frames);
            }
        }

        #region XML

        public override string GetXmlTagName()
        {
            return "StarlingHandler";
        }

        public override XElement ToXml()
        {
            var xml = base.ToXml();
            xml.SetAttributeValue("spriteSheetAnimationSpeed", _spriteSheetAnimationSpeed);
            xml.SetAttributeValue("spriteSheetStartAtRandomFrame", _spriteSheetStartAtRandomFrame);
            xml.SetAttributeValue("smoothing", Smoothing);
            xml.SetAttributeValue("blendMode", _blendMode);
            xml.SetAttributeValue("premultiplyAlpha", _premultiplyAlpha);
            return xml;
        }

        public override void ParseXml(XElement xml, XmlBuilder builder)
        {
            _spriteSheetAnimationSpeed = int.Parse(xml.Attribute("spriteSheetAnimationSpeed").Value);
            _spriteSheetStartAtRandomFrame = bool.Parse(xml.Attribute("spriteSheetStartAtRandomFrame").Value);
            Smoothing = bool.Parse(xml.Attribute("smoothing").Value);
            string bMode = xml.Attribute("blendMode").Value;
            if (bMode == "normal")
            {
                BlendMode = global::Sparrow.Display.BlendMode.NORMAL;   
            }
            else
            {
                throw new NotImplementedException();
            }
            PremultiplyAlpha = bool.Parse(xml.Attribute("premultiplyAlpha").Value);
        }

        #endregion
    }
}