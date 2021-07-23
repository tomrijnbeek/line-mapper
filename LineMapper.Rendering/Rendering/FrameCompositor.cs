using Bearded.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace LineMapper.Rendering.Rendering
{
    public sealed class FrameCompositor
    {
        private Vector2i viewportSize;

        public void SetViewport(Vector2i viewport)
        {
            viewportSize = viewport;
        }

        public void PrepareForFrame()
        {
            var argb = Color.Black;
            GL.ClearColor(argb.R / 255f, argb.G / 255f, argb.B / 255f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Viewport(0, 0, viewportSize.X, viewportSize.Y);
        }

        public void FinalizeFrame() {}
    }
}
