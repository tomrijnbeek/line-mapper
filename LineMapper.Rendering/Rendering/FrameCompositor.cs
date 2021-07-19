using Bearded.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LineMapper.Rendering.Rendering
{
    public sealed class FrameCompositor
    {
        public void PrepareForFrame()
        {
            var argb = Color.Black;
            GL.ClearColor(argb.R / 255f, argb.G / 255f, argb.B / 255f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void FinalizeFrame() {}
    }
}
