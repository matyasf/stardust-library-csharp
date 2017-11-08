
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
using Stardust.Math;
using Stardust.Particles;

namespace Stardust.Handlers.Sparrow
{
    public class SparrowRenderer : DisplayObject
    {
        
        public const int MAX_POSSIBLE_PARTICLES = 16383;
        private static readonly float DEGREES_TO_RADIANS = StardustMath.Pi / 180;
        private static readonly float[] sCosLUT = new float[0x800];
        private static readonly float[] sSinLUT = new float[0x800];
        private static readonly Matrix3D renderMatrix = Matrix3D.Create();
        private static readonly float[] renderAlpha = new float[4];
        private static int numberOfVertexBuffers;
        private static int maxParticles;
        private static bool initCalled = false;
    
        private Rectangle boundsRect;
        private FragmentFilter mFilter;
        private bool mTinted = true;
        private Texture mTexture;
        private bool mBatched;
        private float[] vertexes;
        private List<Frame> frames;

        public int mNumParticles = 0;
        public TextureSmoothing TexSmoothing;
        public bool PremultiplyAlpha = true;

        public SparrowRenderer()
        {
            if (initCalled == false)
            {
                Init();
            }
        }

        /// <summary>
        /// numberOfBuffers is the amount of vertex buffers used by the particle system for multi buffering.
        ///  Multi buffering can avoid stalling of the GPU but will also increases it's memory consumption.
        ///  If you want to avoid stalling create the same amount of buffers as your maximum rendered emitters at the
        /// same time.
        /// Allocating one buffer with the maximum amount of particles (16383) takes up 2048KB(2MB) GPU memory.
        /// This call requires that there is a Starling context
        /// </summary>
        /// <param name="numberOfBuffers">the amount of vertex buffers used by the particle system for multi buffering.</param>
        /// <param name="maxParticlesPerBuffer"></param>
        public static void Init(int numberOfBuffers = 2, int maxParticlesPerBuffer = MAX_POSSIBLE_PARTICLES)
        {
            numberOfVertexBuffers = numberOfBuffers;
            if (maxParticlesPerBuffer > MAX_POSSIBLE_PARTICLES) {
                maxParticlesPerBuffer = MAX_POSSIBLE_PARTICLES;
                Debug.WriteLine("StardustStarlingRenderer WARNING: Tried to render than possible particles, setting value to max");
            }
            maxParticles = maxParticlesPerBuffer;
            SparrowParticleBuffers.CreateBuffers(maxParticlesPerBuffer, numberOfBuffers);

            if (!initCalled) {
                for (int i = 0; i < 0x800; ++i) {
                    sCosLUT[i & 0x7FF] = (float)System.Math.Cos(i * 0.00306796157577128245943617517898); // 0.003067 = 2PI/2048
                    sSinLUT[i & 0x7FF] = (float)System.Math.Sin(i * 0.00306796157577128245943617517898);
                }
                // handle a lost device context
                SparrowSharp.ContextCreated += SparrowSharpOnContextCreated;
                initCalled = true;
            }
        }

        private static void SparrowSharpOnContextCreated()
        {
            SparrowParticleBuffers.CreateBuffers(maxParticles, numberOfVertexBuffers);
        }

        public void SetTextures(Texture texture, List<Frame> _frames)
        {
            mTexture = texture;
            frames = _frames;
        }

        public void AdvanceTime(IList<Particle> mParticles) // TODO make this a native array
        {
            mNumParticles = mParticles.Count;
            vertexes = new float[mNumParticles * 32];
            Particle particle;
            int vertexID = 0;

            float red;
            float green;
            float blue;
            float particleAlpha;

            float rotation;
            float x, y;
            float xOffset, yOffset;

            int angle;
            float cos;
            float sin;
            float cosX;
            float cosY;
            float sinX;
            float sinY;
            int position;
            Frame frame;
            float bottomRightX;
            float bottomRightY;
            float topLeftX;
            float topLeftY;
            for (int i = 0; i < mNumParticles; ++i) {
                vertexID = i << 2;
                particle = mParticles[i];
                // color & alpha
                particleAlpha = particle.Alpha;
                if (PremultiplyAlpha) {
                    red = particle.ColorR * particleAlpha;
                    green = particle.ColorG * particleAlpha;
                    blue = particle.ColorB * particleAlpha;
                }
                else {
                    red = particle.ColorR;
                    green = particle.ColorG;
                    blue = particle.ColorB;
                }
                // position & rotation
                rotation = particle.Rotation * DEGREES_TO_RADIANS;
                x = particle.X;
                y = particle.Y;
                // texture
                frame = frames[particle.CurrentAnimationFrame];
                bottomRightX = frame.BottomRightX;
                bottomRightY = frame.BottomRightY;
                topLeftX = frame.TopLeftX;
                topLeftY = frame.TopLeftY;
                xOffset = frame.ParticleHalfWidth * particle.Scale;
                yOffset = frame.ParticleHalfHeight * particle.Scale;

                position = vertexID << 3; // * 8
                if (rotation != 0) {
                    angle = ((int)(rotation * 325.94932345220164765467394738691f) & 2047);
                    cos = sCosLUT[angle];
                    sin = sSinLUT[angle];
                    cosX = cos * xOffset;
                    cosY = cos * yOffset;
                    sinX = sin * xOffset;
                    sinY = sin * yOffset;
    
                    vertexes[position] = x - cosX + sinY;  // 0,1: position (in pixels)
                    vertexes[++position] = y - sinX - cosY;
                    vertexes[++position] = red;// 2,3,4,5: Color and Alpha [0-1]
                    vertexes[++position] = green;
                    vertexes[++position] = blue;
                    vertexes[++position] = particleAlpha;
                    vertexes[++position] = topLeftX; // 6,7: Texture coords [0-1]
                    vertexes[++position] = topLeftY;
    
                    vertexes[++position] = x + cosX + sinY;
                    vertexes[++position] = y + sinX - cosY;
                    vertexes[++position] = red;
                    vertexes[++position] = green;
                    vertexes[++position] = blue;
                    vertexes[++position] = particleAlpha;
                    vertexes[++position] = bottomRightX;
                    vertexes[++position] = topLeftY;
    
                    vertexes[++position] = x - cosX - sinY;
                    vertexes[++position] = y - sinX + cosY;
                    vertexes[++position] = red;
                    vertexes[++position] = green;
                    vertexes[++position] = blue;
                    vertexes[++position] = particleAlpha;
                    vertexes[++position] = topLeftX;
                    vertexes[++position] = bottomRightY;
    
                    vertexes[++position] = x + cosX - sinY;
                    vertexes[++position] = y + sinX + cosY;
                    vertexes[++position] = red;
                    vertexes[++position] = green;
                    vertexes[++position] = blue;
                    vertexes[++position] = particleAlpha;
                    vertexes[++position] = bottomRightX;
                    vertexes[++position] = bottomRightY;
                }
                else {
                    vertexes[position] = x - xOffset;
                    vertexes[++position] = y - yOffset;
                    vertexes[++position] = red;
                    vertexes[++position] = green;
                    vertexes[++position] = blue;
                    vertexes[++position] = particleAlpha;
                    vertexes[++position] = topLeftX;
                    vertexes[++position] = topLeftY;
    
                    vertexes[++position] = x + xOffset;
                    vertexes[++position] = y - yOffset;
                    vertexes[++position] = red;
                    vertexes[++position] = green;
                    vertexes[++position] = blue;
                    vertexes[++position] = particleAlpha;
                    vertexes[++position] = bottomRightX;
                    vertexes[++position] = topLeftY;
    
                    vertexes[++position] = x - xOffset;
                    vertexes[++position] = y + yOffset;
                    vertexes[++position] = red;
                    vertexes[++position] = green;
                    vertexes[++position] = blue;
                    vertexes[++position] = particleAlpha;
                    vertexes[++position] = topLeftX;
                    vertexes[++position] = bottomRightY;
    
                    vertexes[++position] = x + xOffset;
                    vertexes[++position] = y + yOffset;
                    vertexes[++position] = red;
                    vertexes[++position] = green;
                    vertexes[++position] = blue;
                    vertexes[++position] = particleAlpha;
                    vertexes[++position] = bottomRightX;
                    vertexes[++position] = bottomRightY;
                }
            }
        }
        
        protected bool IsStateChange(uint texture,
                TextureSmoothing smoothing, uint blendMode, FragmentFilter filter,
                bool premultiplyAlpha, int numParticles)
        {
            if (mNumParticles == 0) {
                return false;
            }
            else if (mNumParticles + numParticles > MAX_POSSIBLE_PARTICLES) {
                return true;
            }
            else if (mTexture != null && texture != 0) {
                return mTexture.Base != texture || TexSmoothing != smoothing || BlendMode != blendMode ||
                    mFilter != filter || PremultiplyAlpha != premultiplyAlpha;
            }
            return true;
        }

        public override void Render(Painter painter)
        {
            painter.ExcludeFromCache(this); // for some reason it doesnt work if inside the if. Starling bug?
            if (mNumParticles > 0 && !mBatched) {
                int mNumBatchedParticles = BatchNeighbours();
                float parentAlpha = Parent != null ? Parent.Alpha : 1;
                RenderCustom(painter, mNumBatchedParticles, parentAlpha);
            }
            //reset filter
            base.Filter = mFilter;
            mBatched = false;
        }
        
        protected int BatchNeighbours()
        {
            int mNumBatchedParticles = 0;
            int last = Parent.GetChildIndex(this);
            while (++last < Parent.NumChildren) {
                SparrowRenderer nextPS  = Parent.GetChild(last) as SparrowRenderer;
                if (nextPS != null && !nextPS.IsStateChange(mTexture.Base, TexSmoothing, BlendMode, mFilter, PremultiplyAlpha, mNumParticles)) {
                    if (nextPS.mNumParticles > 0) {
                        int targetIndex = (mNumParticles + mNumBatchedParticles) * 32; // 4 * 8
                        int sourceIndex = 0;
                        int sourceEnd = nextPS.mNumParticles * 32; // 4 * 8
                        while (sourceIndex < sourceEnd) {
                            vertexes[targetIndex++] = nextPS.vertexes[sourceIndex++];
                        }
                        
                        mNumBatchedParticles += nextPS.mNumParticles;
                        
                        nextPS.mBatched = true;

                        //disable filter of batched system temporarily
                        nextPS.Filter = null;
                    }
                }
                else {
                    break;
                }
            }
            return mNumBatchedParticles;
        }
        
        private void RenderCustom(Painter painter, int mNumBatchedParticles, float parentAlpha)
        {
            if (mNumParticles == 0 || SparrowParticleBuffers.BuffersCreated == false) {
                return;
            }
            if (mNumBatchedParticles > maxParticles) {
                Debug.WriteLine("Over " + maxParticles + " particles! Aborting rendering");
                return;
            }
            SparrowParticleBuffers.SwitchVertexBuffer();
    
            painter.FinishMeshBatch();
            painter.DrawCount += 1;
            painter.PrepareToDraw();
            
            global::Sparrow.Display.BlendMode.Get(BlendMode).Activate();
    
            renderAlpha[0] = renderAlpha[1] = renderAlpha[2] = PremultiplyAlpha ? parentAlpha : 1;
            renderAlpha[3] = parentAlpha;

            var program = ParticleProgram.GetProgram();
            program.Activate();
              
            //context.setProgramConstantsFromVector(Context3DProgramType.VERTEX, 0, renderAlpha, 1);
            //context.setProgramConstantsFromMatrix(Context3DProgramType.VERTEX, 1, painter.State.MvpMatrix3D, true);
            
            int uAlpha = program.Uniforms["uAlpha"];
            Gl.Uniform4(uAlpha, renderAlpha[0], renderAlpha[1], renderAlpha[2], renderAlpha[3]);
            
            int uMvpMatrix = program.Uniforms["uMvpMatrix"];
            Gl.UniformMatrix4(uMvpMatrix, 1, false, painter.State.MvpMatrix3D.RawData);
            
            //context.setTextureAt(0, mTexture.Base);

            //SparrowParticleBuffers.VertexBuffer.UploadFromVector(vertexes, 0, System.Math.Min(maxParticles * 4, vertexes.Count / 8));
            //context.setVertexBufferAt(0, SparrowParticleBuffers.VertexBuffer, POSITION_OFFSET, Context3DVertexBufferFormat.FLOAT_2);
            //context.setVertexBufferAt(1, SparrowParticleBuffers.VertexBuffer, COLOR_OFFSET, Context3DVertexBufferFormat.FLOAT_4);
            //context.setVertexBufferAt(2, SparrowParticleBuffers.VertexBuffer, TEXCOORD_OFFSET, Context3DVertexBufferFormat.FLOAT_2);
            Gl.BindBuffer(BufferTarget.ArrayBuffer, SparrowParticleBuffers.VertexBuffer);
            Gl.BufferData(BufferTarget.ArrayBuffer, (uint)(vertexes.Length * sizeof(float)), vertexes, BufferUsage.DynamicDraw);
            
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
            RenderUtil.SetSamplerStateAt(mTexture.Base, mTexture.NumMipMaps > 0, TexSmoothing);
            
            
            //context.drawTriangles(SparrowParticleBuffers.IndexBuffer, 0, (System.Math.Min(maxParticles, mNumParticles + mNumBatchedParticles)) * 2);
            Gl.DrawElements(PrimitiveType.Triangles, (mNumParticles + mNumBatchedParticles) * 3, DrawElementsType.UnsignedShort, IntPtr.Zero);

            //context.setVertexBufferAt(0, null);
            //context.setVertexBufferAt(1, null);
            //context.setVertexBufferAt(2, null);
            //context.setTextureAt(0, null);
        }

        public override FragmentFilter Filter
        {
            get => base.Filter;
            set
            {
                if (!mBatched)
                {
                    mFilter = value;
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
            if (boundsRect == null)
            {
                boundsRect = Rectangle.Create();
            }
            return boundsRect;
        }


    }
}