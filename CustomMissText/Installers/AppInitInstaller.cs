using IPA.Logging;
using SiraUtil;
using Zenject;

namespace CustomMissText.Installers
{
    public class AppInitInstaller : Installer<AppInitInstaller>
    {
        readonly Logger _logger;
        readonly PluginConfig _config;

        public AppInitInstaller(Logger logger, PluginConfig config)
        {
            _logger = logger;
            _config = config;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ListBuilder>().AsSingle();
            Container.Bind<PluginConfig>().FromInstance(_config).AsCached();
            Container.BindLoggerAsSiraLogger(_logger);
        }
    }
}
