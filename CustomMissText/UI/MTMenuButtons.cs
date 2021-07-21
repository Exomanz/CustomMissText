using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using System;
using Zenject;

namespace CustomMissText.UI
{
    internal class MTMenuButtons : IInitializable, IDisposable
    {
        MainFlowCoordinator _mainFC;
        MTFlowCoord _modFC;
        MenuButton _button;

        public MTMenuButtons(MainFlowCoordinator main, MTFlowCoord mod)
        {
            _mainFC = main;
            _modFC = mod;
            _button = new MenuButton("CustomMissText", SummonFlowCoordinator);
        }

        public void Initialize() => MenuButtons.instance.RegisterButton(_button);

        internal void SummonFlowCoordinator() =>
            _mainFC.PresentFlowCoordinator(_modFC, null, HMUI.ViewController.AnimationDirection.Horizontal);

        public void Dispose()
        {
            if (BSMLParser.IsSingletonAvailable && MenuButtons.IsSingletonAvailable)
                MenuButtons.instance.UnregisterButton(_button);
        }
    }
}
