using System.Collections.Generic;

namespace Stardust.Handlers.Sparrow
{
    public class SparrowParticleBuffers
    {

        public static IndexBuffer3D indexBuffer;
        protected static List<VertexBuffer3D> vertexBuffers;
        private static List<int> indices;
        protected static int sNumberOfVertexBuffers;
        private static int _vertexBufferIdx = -1;
        
        /// <summary>
        /// Creates buffers for the simulation.
        /// numberOfBuffers is the amount of vertex buffers used by the particle system for multi buffering. Multi buffering
        /// can avoid stalling of the GPU but will also increases it's memory consumption.
        /// </summary>
        public static void CreateBuffers(int numParticles, int numberOfVertexBuffers)
        {
            
        }
        
        /// <summary>
        /// Call this function to switch to the next Vertex buffer before calling uploadFromVector() to implement multi
        /// buffering. Has only effect if numberOfVertexBuffers > 1
        /// </summary>
        public static void SwitchVertexBuffer()
        {
            _vertexBufferIdx = ++_vertexBufferIdx % sNumberOfVertexBuffers;
        }

        public static VertexBuffer3D VertexBuffer
        {
            get => vertexBuffers[_vertexBufferIdx];
        }
        
        public static int VertexBufferIdx => _vertexBufferIdx;

        public static bool BuffersCreated
        {
            get
            {
                if (vertexBuffers != null && vertexBuffers.Count > 0) {
                    return true;
                }
                return false;
            }
        }
    }
}