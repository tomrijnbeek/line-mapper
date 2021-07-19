using LineMapper.Rendering.Shaders;

namespace LineMapper.Rendering.Rendering
{
    public sealed class RenderContext
    {
        public static RenderContext Load()
        {
            var shaders = CoreShaders.Load();
            var renderers = new CoreRenderers(shaders);
            var drawers = new CoreDrawers(renderers);

            return new RenderContext(shaders, renderers, drawers);
        }

        public CoreShaders Shaders { get; }
        public CoreRenderers Renderers { get; }
        public CoreDrawers Drawers { get; }

        private RenderContext(CoreShaders shaders, CoreRenderers renderers, CoreDrawers drawers)
        {
            Shaders = shaders;
            Renderers = renderers;
            Drawers = drawers;
        }
    }
}
