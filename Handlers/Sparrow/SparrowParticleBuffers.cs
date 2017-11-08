using System;
using OpenGL;

namespace Stardust.Handlers.Sparrow
{
    public class SparrowParticleBuffers
    {

        public static uint IndexBuffer;
        protected static uint[] VertexBuffers;
        private static int[] _indices;
        protected static int SNumberOfVertexBuffers;
        private static int _vertexBufferIdx = -1;
        
        /// <summary>
        /// Creates buffers for the simulation.
        /// numberOfBuffers is the amount of vertex buffers used by the particle system for multi buffering. Multi buffering
        /// can avoid stalling of the GPU but will also increases it's memory consumption.
        /// </summary>
        public static void CreateBuffers(int numParticles, int numberOfVertexBuffers)
        {
            SNumberOfVertexBuffers = numberOfVertexBuffers;
            _vertexBufferIdx = -1;
            if (VertexBuffers != null)
            {
                Gl.DeleteBuffers(VertexBuffers);
            }
            if (IndexBuffer != 0)
            {
                Gl.DeleteBuffers(IndexBuffer);
            }
            
            VertexBuffers = new uint[SNumberOfVertexBuffers];
            for (int i = 0; i < SNumberOfVertexBuffers; ++i) 
            {
                //vertexBuffers[i] = context.createVertexBuffer(context, numParticles * 4, ELEMENTS_PER_VERTEX, "DynamicDraw");
                VertexBuffers[i] = Gl.GenBuffer();
            }
    
            //ByteArray zeroBytes = new ByteArray();
            //zeroBytes.length = numParticles * 16 * ELEMENTS_PER_VERTEX;
            for (int i = 0; i < SNumberOfVertexBuffers; ++i)
            {
                //vertexBuffers[i].uploadFromByteArray(zeroBytes, 0, 0, numParticles * 4);
                Gl.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffers[i]);
                Gl.BufferData(BufferTarget.ArrayBuffer, (uint)(numParticles * 4 * sizeof(float)), IntPtr.Zero, BufferUsage.DynamicDraw);
            }
            //zeroBytes.length = 0;
    
            if (_indices == null)
            {
                _indices = new int[numParticles * 6];
                int numVertices = 0;
                int indexPosition = -1;
                for (int i = 0; i < numParticles; ++i)
                {
                    _indices[++indexPosition] = numVertices;
                    _indices[++indexPosition] = numVertices + 1;
                    _indices[++indexPosition] = numVertices + 2;
    
                    _indices[++indexPosition] = numVertices + 1;
                    _indices[++indexPosition] = numVertices + 3;
                    _indices[++indexPosition] = numVertices + 2;
                    numVertices += 4;
                }
            }
            //indexBuffer = context.createIndexBuffer(numParticles * 6);
            //indexBuffer.uploadFromVector(indices, 0, numParticles * 6);
            IndexBuffer = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
            Gl.BufferData(BufferTarget.ElementArrayBuffer, (uint)(sizeof(short) * numParticles * 6), _indices, BufferUsage.DynamicDraw);
        }
        
        /// <summary>
        /// Call this function to switch to the next Vertex buffer before calling Gl.BufferData() to implement multi
        /// buffering. Has only effect if numberOfVertexBuffers > 1
        /// </summary>
        public static void SwitchVertexBuffer()
        {
            _vertexBufferIdx = ++_vertexBufferIdx % SNumberOfVertexBuffers;
        }

        public static uint VertexBuffer
        {
            get => VertexBuffers[_vertexBufferIdx];
        }
        
        public static int VertexBufferIdx => _vertexBufferIdx;

        public static bool BuffersCreated
        {
            get
            {
                if (VertexBuffers != null && VertexBuffers.Length > 0) {
                    return true;
                }
                return false;
            }
        }
    }
}