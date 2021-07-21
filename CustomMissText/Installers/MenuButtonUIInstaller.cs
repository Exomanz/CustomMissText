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
            Container.Bind<MTFlowCoord>().FromNewComponentOnNewGameObject(nameof(MTFlowCoord)).AsSingle();
            Container.Bind<MTSettingsView>().FromNewComponentAsViewController().AsSingle();

            Container.BindInterfacesAndSelfTo<MTMenuButtons>().AsSingle();
        }
    }
}
