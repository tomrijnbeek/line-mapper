using System;
using System.IO;
using Bearded.Graphics.ShaderManagement;

namespace LineMapper.Rendering.Shaders
{
    public sealed class CoreShaders
    {
        public static CoreShaders Load()
        {
            var workingDir = Directory.GetCurrentDirectory() + "/";
            var shadersDir = workingDir + "Shaders/";

            var shaderFiles = ShaderFileLoader.CreateDefault(shadersDir).Load(".");
            var shaders = new ShaderManager();
            shaders.AddRange(shaderFiles);

            shaders.RegisterRendererShaderFromAllShadersWithName("geometry");

            return new CoreShaders(shaders);
        }

        private readonly ShaderManager shaders;

        private CoreShaders(ShaderManager shaders)
        {
            this.shaders = shaders;
        }

        public IRendererShader GetRendererShader(string name)
        {
            return shaders.TryGetRendererShader(name, out var program)
                ? program
                : throw new ArgumentException($"Shader {name} not found.", nameof(name));
        }
    }
}
