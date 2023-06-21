using System;
using Bearded.Graphics;
using Bearded.Graphics.Windowing;
using Bearded.UI.Controls;
using Bearded.UI.Rendering;
using Bearded.Utilities.IO;
using LineMapper.Rendering.Rendering;
using LineMapper.UI;
using LineMapper.UI.Controls;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace LineMapper;

sealed class ProgramWindow : Window
{
    private readonly Logger logger;
    private RenderContext renderContext = null!;
    private IRendererRouter rendererRouter = null!;
    private RootControl rootControl = null!;
    // TODO: this shouldn't be here
    private Camera camera = null!;

    public ProgramWindow(Logger logger)
    {
        this.logger = logger;
    }

    protected override NativeWindowSettings GetSettings() =>
        new()
        {
            // TODO: consider event driven window
            API = ContextAPI.OpenGL,
            APIVersion = new Version(3, 2),
            Size = new Vector2i(1280, 720),
            Title = "Line Mapper",
            WindowState = WindowState.Normal,
            NumberOfSamples = 8,
        };

    protected override void OnLoad()
    {
        renderContext = RenderContext.Load();

        rendererRouter = ControlLibrary.InitializeRenderers(renderContext);
        rootControl = new RootControl();
        rootControl.Add(new LayoutControl());

        camera = new Camera(logger);
    }

    protected override void OnResize(ResizeEventArgs eventArgs)
    {
        renderContext.Compositor.SetViewport(eventArgs.Size);
        rootControl.SetViewport(eventArgs.Width, eventArgs.Height, 1);
        camera.ResizeViewport(eventArgs.Size);
    }

    protected override void OnUpdate(UpdateEventArgs e)
    {
    }

    protected override void OnRender(UpdateEventArgs e)
    {
        camera.ApplyTo(renderContext.Settings);

        renderContext.Compositor.PrepareForFrame();
        renderContext.Renderers.ClearAll();
        rootControl.Render(rendererRouter);
        renderContext.Renderers.RenderAll();
        renderContext.Compositor.FinalizeFrame();

        SwapBuffers();
    }
}
