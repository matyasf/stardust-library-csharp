
using OpenGL;

namespace Stardust.Sparrow
{
    public static class SparrowParticleBuffers
    {

        public static uint IndexBuffer;
        private static uint[] _vertexBuffers;
        private static ushort[] _indices;
        private static int _sNumberOfVertexBuffers;
        private static int _vertexBufferIdx = -1;
        
        /// <summary>
        /// Creates buffers for the simulation.
        /// numberOfBuffers is the amount of vertex buffers used by the particle system for multi buffering. 
        /// Multi buffering can avoid stalling of the GPU but will also increases it's memory consumption.
        /// </summary>
        public static void CreateBuffers(int maxNumParticles, int numberOfVertexBuffers)
        {
            _sNumberOfVertexBuffers = numberOfVertexBuffers;
            _vertexBufferIdx = -1;
            if (_vertexBuffers != null)
            {
                Gl.DeleteBuffers(_vertexBuffers);
            }
            if (IndexBuffer != 0)
            {
                Gl.DeleteBuffers(IndexBuffer);
            }
            
            _vertexBuffers = new uint[_sNumberOfVertexBuffers];
            for (int i = 0; i < _sNumberOfVertexBuffers; ++i) 
            {
                _vertexBuffers[i] = Gl.GenBuffer();
            }
    
            if (_indices == null)
            {
                _indices = new ushort[maxNumParticles * 6];
                short numVertices = 0;
                int indexPosition = -1;
                for (int i = 0; i < maxNumParticles; ++i)
                {
                    _indices[++indexPosition] = (ushort) numVertices;
                    _indices[++indexPosition] = (ushort) (numVertices + 1);
                    _indices[++indexPosition] = (ushort) (numVertices + 2);
    
                    _indices[++indexPosition] = (ushort) (numVertices + 1);
                    _indices[++indexPosition] = (ushort) (numVertices + 3);
                    _indices[++indexPosition] = (ushort) (numVertices + 2);
                    numVertices += 4;
                }
            }
            IndexBuffer = Gl.GenBuffer();
            Gl.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBuffer);
            Gl.BufferData(BufferTarget.ElementArrayBuffer, (uint)(_indices.Length * sizeof(ushort)), _indices, BufferUsage.DynamicDraw);
        }
        
        /// <summary>
        /// Call this function to switch to the next Vertex buffer before calling Gl.BufferData() to implement multi
        /// buffering. Has only effect if numberOfVertexBuffers > 1
        /// </summary>
        public static void SwitchVertexBuffer()
        {
            _vertexBufferIdx = ++_vertexBufferIdx % _sNumberOfVertexBuffers;
        }

        public static uint VertexBuffer => _vertexBuffers[_vertexBufferIdx];

        public static int VertexBufferIdx => _vertexBufferIdx;

        public static bool BuffersCreated
        {
            get
            {
                if (_vertexBuffers != null && _vertexBuffers.Length > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}