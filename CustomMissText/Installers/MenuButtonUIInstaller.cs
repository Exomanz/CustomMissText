using CustomMissText.UI;
using CustomMissText.UI.Settings;
using SiraUtil;
using Zenject;

namespace CustomMissText.Installers
{
    public class MenuButtonUIInstaller : Installer<MenuButtonUIInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ModFlowCoordinator>().FromNewComponentOnNewGameObject(nameof(ModFlowCoordinator)).AsSingle();
            Container.Bind<MissTextMainSettingsController>().FromNewComponentAsViewController().AsSingle();

            Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle();
        }
    }
}
