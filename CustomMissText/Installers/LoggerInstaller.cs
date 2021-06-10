using IPA.Logging;
using SiraUtil;
using Zenject;

namespace CustomMissText.Installers
{
    public class LoggerInstaller : Installer<LoggerInstaller>
    {
        readonly Logger _logger;
        readonly PluginConfig _config;

        public LoggerInstaller(Logger logger, PluginConfig config)
        {
            _logger = logger;
            _config = config;
        }

        public override void InstallBindings()
        {
            Container.Bind<PluginConfig>().FromInstance(_config).AsCached();
            Container.BindLoggerAsSiraLogger(_logger);
        }
    }
}
