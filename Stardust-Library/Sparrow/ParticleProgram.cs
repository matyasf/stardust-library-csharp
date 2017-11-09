
using Sparrow.Rendering;

namespace Stardust.Sparrow
{
    public static class ParticleProgram
    {

        private static Program _program;
        
        public static Program GetProgram()
        {
            if (_program == null)
            {
                var vertexShader = Effect.AddShaderInitCode() + @"
                    in vec4 aPosition;
                    in vec4 aColor;
                    in vec2 aTexCoords;
    
                    uniform mat4 uMvpMatrix;
                    uniform vec4 uAlpha;
    
                    out lowp vec4 vColor;
                    out lowp vec2 vTexCoords;
                    
                    void main() {
                      gl_Position = uMvpMatrix * aPosition;
                      vColor = aColor * uAlpha;
                      vTexCoords  = aTexCoords;
                    }";
                
                var fragmentShader = Effect.AddShaderInitCode() + @"
                    in lowp vec4 vColor;
                    in lowp vec2 vTexCoords;
                    uniform lowp sampler2D uTexture;
                    out lowp vec4 fragColor;
                    
                    void main() {
                      fragColor = texture(uTexture, vTexCoords) * vColor;
                    }";
                _program = new Program(vertexShader, fragmentShader);   
            }
            return _program;
        }
    }
}