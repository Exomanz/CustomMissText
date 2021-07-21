using BeatSaberMarkupLanguage;
using CustomMissText.UI.Settings;
using HMUI;
using Zenject;

namespace CustomMissText.UI
{
    internal class MTFlowCoord : FlowCoordinator
    {
        MainFlowCoordinator _mainFlow;
        MTSettingsView _settings;

        [Inject]
        public void Construct(MainFlowCoordinator main, MTSettingsView settings)
        {
            _mainFlow = main;
            _settings = settings;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("CustomMissText", ViewController.AnimationType.In);
                showBackButton = true;
                ProvideInitialViewControllers(_settings);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            base.BackButtonWasPressed(topViewController);
            _mainFlow.DismissFlowCoordinator(this, null, ViewController.AnimationDirection.Vertical);
        }
    }
}
