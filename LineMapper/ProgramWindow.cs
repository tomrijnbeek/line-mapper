using System;
using Bearded.Graphics;
using Bearded.Graphics.Windowing;
using Bearded.Utilities.IO;
using LineMapper.UI.Rendering;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace LineMapper
{
    sealed class ProgramWindow : Window
    {
        private readonly Logger logger;
        private RenderContext renderContext = null!;

        public ProgramWindow(Logger logger)
        {
            this.logger = logger;
        }

        protected override NativeWindowSettings GetSettings() =>
            new()
            {
                API = ContextAPI.OpenGL,
                APIVersion = new Version(3, 2),
                Size = new Vector2i(1280, 720),
                Title = "Line Mapper",
                WindowState = WindowState.Normal
            };

        protected override void OnLoad()
        {
            renderContext = RenderContext.Load();
        }

        protected override void OnResize(ResizeEventArgs eventArgs)
        {
            GL.Viewport(0, 0, eventArgs.Width, eventArgs.Height);
        }

        protected override void OnUpdate(UpdateEventArgs e)
        {
        }

        protected override void OnRender(UpdateEventArgs e)
        {
        }
    }
}
