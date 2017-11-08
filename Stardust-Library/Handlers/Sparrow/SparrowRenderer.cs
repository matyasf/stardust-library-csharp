
using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenGL;
using Sparrow.Core;
using Sparrow.Display;
using Sparrow.Filters;
using Sparrow.Geom;
using Sparrow.Rendering;
using Sparrow.Textures;
using Sparrow.Utils;
using Stardust.MathStuff;
using Stardust.Particles;

namespace Stardust.Handlers.Sparrow
{
    public class SparrowRenderer : DisplayObject
    {
        
        public const int MaxPossibleParticles = 50000;
        private const float DegreesToRadians = StardustMath.Pi / 180;
        private static readonly float[] SCosLut = new float[0x800];
        private static readonly float[] SSinLut = new float[0x800];
        private static readonly float[] RenderAlpha = new float[4];
        private static int _numberOfVertexBuffers;
        private static int _maxParticles;
        private static bool _initCalled;
    
        private Rectangle _boundsRect;
        private FragmentFilter _mFilter;
        private Texture _mTexture;
        private bool _mBatched;
        private float[] _vertexes;
        private List<Frame> _frames;

        private int _mNumParticles;
        public TextureSmoothing TexSmoothing;
        public bool PremultiplyAlpha = true;

        public SparrowRenderer()
        {
            if (_initCalled == false)
            {
                Init();
            }
        }

        /// <summary>
        /// numberOfBuffers is the amount of vertex buffers used by the particle system for multi buffering.
        /// Multi buffering can avoid stalling of the GPU but will also increases it's memory consumption.
        /// If you want to avoid stalling create the same amount of buffers as your maximum rendered emitters at the
        /// same time.
        /// For example allocating one buffer with 16383 particles takes up 2048KB(2MB) GPU memory.
        /// This call requires that there is a GL context
        /// </summary>
        /// <param name="numberOfBuffers">the amount of vertex buffers used by the particle system for multi buffering.</param>
        /// <param name="maxParticlesPerBuffer">Maximum number of particles that you will be able to display.</param>
        public static void Init(int numberOfBuffers = 2, int maxParticlesPerBuffer = MaxPossibleParticles)
        {
            _numberOfVertexBuffers = numberOfBuffers;
            if (maxParticlesPerBuffer > MaxPossibleParticles) 
            {
                maxParticlesPerBuffer = MaxPossibleParticles;
                Debug.WriteLine("StardustStarlingRenderer WARNING: Tried to render than possible particles, setting value to max");
            }
            _maxParticles = maxParticlesPerBuffer;
            SparrowParticleBuffers.CreateBuffers(maxParticlesPerBuffer, numberOfBuffers);

            if (!_initCalled)
            {
                for (int i = 0; i < 0x800; ++i) 
                {
                    SCosLut[i & 0x7FF] = (float)Math.Cos(i * 0.00306796157577128245943617517898); // 0.003067 = 2PI/2048
                    SSinLut[i & 0x7FF] = (float)Math.Sin(i * 0.00306796157577128245943617517898);
                }
                // handle a lost device context
                SparrowSharp.ContextCreated += SparrowSharpOnContextCreated;
                _initCalled = true;
            }
        }

        private static void SparrowSharpOnContextCreated()
        {
            SparrowParticleBuffers.CreateBuffers(_maxParticles, _numberOfVertexBuffers);
        }

        public void SetTextures(Texture texture, List<Frame> frames)
        {
            _mTexture = texture;
            _frames = frames;
        }

        public void AdvanceTime(IList<Particle> mParticles) // TODO make this a native array
        {
            _mNumParticles = mParticles.Count;
            _vertexes = new float[_mNumParticles * 32];

            for (int i = 0; i < _mNumParticles; ++i) 
            {
                var vertexId = i << 2;
                var particle = mParticles[i];
                // color & alpha
                var particleAlpha = particle.Alpha;
                float red;
                float green;
                float blue;
                if (PremultiplyAlpha) 
                {
                    red = particle.ColorR * particleAlpha;
                    green = particle.ColorG * particleAlpha;
                    blue = particle.ColorB * particleAlpha;
                }
                else 
                {
                    red = particle.ColorR;
                    green = particle.ColorG;
                    blue = particle.ColorB;
                }
                // position & rotation
                var rotation = particle.Rotation * DegreesToRadians;
                var x = particle.X;
                var y = particle.Y;
                // texture
                var frame = _frames[particle.CurrentAnimationFrame];
                var bottomRightX = frame.BottomRightX;
                var bottomRightY = frame.BottomRightY;
                var topLeftX = frame.TopLeftX;
                var topLeftY = frame.TopLeftY;
                var xOffset = frame.ParticleHalfWidth * particle.Scale;
                var yOffset = frame.ParticleHalfHeight * particle.Scale;

                var position = vertexId << 3;
                if (rotation != 0f)
                {
                    var angle = ((int)(rotation * 325.94932345220164765467394738691f) & 2047);
                    var cos = SCosLut[angle];
                    var sin = SSinLut[angle];
                    var cosX = cos * xOffset;
                    var cosY = cos * yOffset;
                    var sinX = sin * xOffset;
                    var sinY = sin * yOffset;
    
                    _vertexes[position] = x - cosX + sinY;  // 0,1: position (in pixels)
                    _vertexes[++position] = y - sinX - cosY;
                    _vertexes[++position] = red;// 2,3,4,5: Color and Alpha [0-1]
                    _vertexes[++position] = green;
                    _vertexes[++position] = blue;
                    _vertexes[++position] = particleAlpha;
                    _vertexes[++position] = topLeftX; // 6,7: Texture coords [0-1]
                    _vertexes[++position] = topLeftY;
    
                    _vertexes[++position] = x + cosX + sinY;
                    _vertexes[++position] = y + sinX - cosY;
                    _vertexes[++position] = red;
                    _vertexes[++position] = green;
                    _vertexes[++position] = blue;
                    _vertexes[++position] = particleAlpha;
                    _vertexes[++position] = bottomRightX;
                    _vertexes[++position] = topLeftY;
    
                    _vertexes[++position] = x - cosX - sinY;
                    _vertexes[++position] = y - sinX + cosY;
                    _vertexes[++position] = red;
                    _vertexes[++position] = green;
                    _vertexes[++position] = blue;
                    _vertexes[++position] = particleAlpha;
                    _vertexes[++position] = topLeftX;
                    _vertexes[++position] = bottomRightY;
    
                    _vertexes[++position] = x + cosX - sinY;
                    _vertexes[++position] = y + sinX + cosY;
                    _vertexes[++position] = red;
                    _vertexes[++position] = green;
                    _vertexes[++position] = blue;
                    _vertexes[++position] = particleAlpha;
                    _vertexes[++position] = bottomRightX;
                    _vertexes[++position] = bottomRightY;
                }
                else
                {
                    _vertexes[position] = x - xOffset;
                    _vertexes[++position] = y - yOffset;
                    _vertexes[++position] = red;
                    _vertexes[++position] = green;
                    _vertexes[++position] = blue;
                    _vertexes[++position] = particleAlpha;
                    _vertexes[++position] = topLeftX;
                    _vertexes[++position] = topLeftY;
    
                    _vertexes[++position] = x + xOffset;
                    _vertexes[++position] = y - yOffset;
                    _vertexes[++position] = red;
                    _vertexes[++position] = green;
                    _vertexes[++position] = blue;
                    _vertexes[++position] = particleAlpha;
                    _vertexes[++position] = bottomRightX;
                    _vertexes[++position] = topLeftY;
    
                    _vertexes[++position] = x - xOffset;
                    _vertexes[++position] = y + yOffset;
                    _vertexes[++position] = red;
                    _vertexes[++position] = green;
                    _vertexes[++position] = blue;
                    _vertexes[++position] = particleAlpha;
                    _vertexes[++position] = topLeftX;
                    _vertexes[++position] = bottomRightY;
    
                    _vertexes[++position] = x + xOffset;
                    _vertexes[++position] = y + yOffset;
                    _vertexes[++position] = red;
                    _vertexes[++position] = green;
                    _vertexes[++position] = blue;
                    _vertexes[++position] = particleAlpha;
                    _vertexes[++position] = bottomRightX;
                    _vertexes[++position] = bottomRightY;
                }
            }
        }
        
        protected bool IsStateChange(uint texture,
                TextureSmoothing smoothing, uint blendMode, FragmentFilter filter,
                bool premultiplyAlpha, int numParticles)
        {
            if (_mNumParticles == 0)
            {
                return false;
            }
            if (_mNumParticles + numParticles > MaxPossibleParticles)
            {
                return true;
            }
            if (_mTexture != null && texture != 0)
            {
                return _mTexture.Base != texture || TexSmoothing != smoothing || BlendMode != blendMode ||
                       _mFilter != filter || PremultiplyAlpha != premultiplyAlpha;
            }
            return true;
        }

        public override void Render(Painter painter)
        {
            painter.ExcludeFromCache(this); // for some reason it doesnt work if inside the if. Starling bug?
            if (_mNumParticles > 0 && !_mBatched)
            {
                int mNumBatchedParticles = BatchNeighbours();
                float parentAlpha = Parent != null ? Parent.Alpha : 1;
                RenderCustom(painter, mNumBatchedParticles, parentAlpha);
            }
            //reset filter
            base.Filter = _mFilter;
            _mBatched = false;
        }
        
        protected int BatchNeighbours()
        {
            int mNumBatchedParticles = 0;
            int last = Parent.GetChildIndex(this);
            while (++last < Parent.NumChildren)
            {
                SparrowRenderer nextPs  = Parent.GetChild(last) as SparrowRenderer;
                if (nextPs != null && !nextPs.IsStateChange(_mTexture.Base, TexSmoothing, BlendMode, _mFilter, PremultiplyAlpha, _mNumParticles))
                {
                    if (nextPs._mNumParticles > 0)
                    {
                        int targetIndex = (_mNumParticles + mNumBatchedParticles) * 32; // 4 * 8
                        int sourceIndex = 0;
                        int sourceEnd = nextPs._mNumParticles * 32; // 4 * 8
                        while (sourceIndex < sourceEnd)
                        {
                            _vertexes[targetIndex++] = nextPs._vertexes[sourceIndex++];
                        }
                        mNumBatchedParticles += nextPs._mNumParticles;
                        nextPs._mBatched = true;
                        //disable filter of batched system temporarily
                        nextPs.Filter = null;
                    }
                }
                else 
                {
                    break;
                }
            }
            return mNumBatchedParticles;
        }
        
        private void RenderCustom(Painter painter, int mNumBatchedParticles, float parentAlpha)
        {
            if (_mNumParticles == 0 || SparrowParticleBuffers.BuffersCreated == false)
            {
                return;
            }
            if (mNumBatchedParticles > _maxParticles)
            {
                Debug.WriteLine("Over " + _maxParticles + " particles! Aborting rendering");
                return;
            }
            SparrowParticleBuffers.SwitchVertexBuffer();
    
            painter.FinishMeshBatch();
            painter.DrawCount += 1;
            painter.PrepareToDraw();
            
            global::Sparrow.Display.BlendMode.Get(BlendMode).Activate();
    
            RenderAlpha[0] = RenderAlpha[1] = RenderAlpha[2] = PremultiplyAlpha ? parentAlpha : 1;
            RenderAlpha[3] = parentAlpha;

            var program = ParticleProgram.GetProgram();
            program.Activate();
              
            int uAlpha = program.Uniforms["uAlpha"];
            Gl.Uniform4(uAlpha, RenderAlpha[0], RenderAlpha[1], RenderAlpha[2], RenderAlpha[3]);
            
            int uMvpMatrix = program.Uniforms["uMvpMatrix"];
            Gl.UniformMatrix4(uMvpMatrix, 1, false, painter.State.MvpMatrix3D.RawData);
            
            //context.setTextureAt(0, mTexture.Base);

            Gl.BindBuffer(BufferTarget.ArrayBuffer, SparrowParticleBuffers.VertexBuffer);
            Gl.BufferData(BufferTarget.ArrayBuffer, (uint)(_vertexes.Length * sizeof(float)), _vertexes, BufferUsage.DynamicDraw);
            
            uint attribPosition = (uint)program.Attributes["aPosition"];
            Gl.EnableVertexAttribArray(attribPosition);
            Gl.VertexAttribPointer(attribPosition, 2, VertexAttribType.Float, false, 32, (IntPtr)0);
            
            uint attribColor = (uint)program.Attributes["aColor"];
            Gl.EnableVertexAttribArray(attribColor);
            Gl.VertexAttribPointer(attribColor, 4, VertexAttribType.Float, false, 32, (IntPtr)8);
            
            uint aTexCoords = (uint)program.Attributes["aTexCoords"];
            Gl.EnableVertexAttribArray(aTexCoords);
            Gl.VertexAttribPointer(aTexCoords, 2, VertexAttribType.Float, false, 32, (IntPtr)24);
            Gl.ActiveTexture(TextureUnit.Texture0);       
            RenderUtil.SetSamplerStateAt(_mTexture.Base, _mTexture.NumMipMaps > 0, TexSmoothing);
            
            Gl.BindBuffer(BufferTarget.ElementArrayBuffer, SparrowParticleBuffers.IndexBuffer);
            
            // TODO limit max number of particles
            Gl.DrawElements(PrimitiveType.Triangles, (_mNumParticles + mNumBatchedParticles) * 6, DrawElementsType.UnsignedShort, IntPtr.Zero);

            Gl.DisableVertexAttribArray(attribPosition);
            Gl.DisableVertexAttribArray(attribColor);
            Gl.DisableVertexAttribArray(aTexCoords);
            Gl.BindTexture(TextureTarget.Texture2d, 0);
        }

        public override FragmentFilter Filter
        {
            get => base.Filter;
            set
            {
                if (!_mBatched)
                {
                    _mFilter = value;
                }
                base.Filter = value;
            }
        }

        /// <summary>
        /// Stardust does not calculate the bounds of the simulation. In the future this would be possible, but
        /// will be a performance heavy operation.
        /// </summary>
        public override Rectangle GetBounds(DisplayObject targetSpace)
        {
            if (_boundsRect == null)
            {
                _boundsRect = Rectangle.Create();
            }
            return _boundsRect;
        }


    }
}