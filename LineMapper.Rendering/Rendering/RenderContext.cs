using LineMapper.Rendering.Shaders;

namespace LineMapper.Rendering.Rendering
{
    public sealed class RenderContext
    {
        public static RenderContext Load()
        {
            var shaders = CoreShaders.Load();
            var settings = new SharedRenderSettings();
            var renderers = new CoreRenderers(shaders, settings);
            var drawers = new CoreDrawers(renderers);

            return new RenderContext(shaders, settings, renderers, drawers);
        }

        public CoreShaders Shaders { get; }
        public SharedRenderSettings Settings { get; }
        public CoreRenderers Renderers { get; }
        public CoreDrawers Drawers { get; }
        public FrameCompositor Compositor { get; }

        private RenderContext(
            CoreShaders shaders, SharedRenderSettings settings, CoreRenderers renderers, CoreDrawers drawers)
        {
            Shaders = shaders;
            Renderers = renderers;
            Drawers = drawers;
            Settings = settings;
            Compositor = new FrameCompositor();
        }
    }
}
