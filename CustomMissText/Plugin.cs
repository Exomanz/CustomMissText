using BeatSaberMarkupLanguage.Settings;
using CustomMissText.Services;
using CustomMissText.UI;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using IPAConfig = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;
using System.Reflection;
using UnityEngine;

namespace CustomMissText
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static IPALogger Logger { get; private set; }
        internal static PluginConfig Config { get; private set; }

        private static Harmony harmony;
        private EntryFileReader fileReader;

        [Init] 
        public Plugin(IPALogger logger, IPAConfig config)
        {
            harmony = new Harmony(Helpers.HARMONY_ID);
            Config = config.Generated<PluginConfig>();
            Logger = logger;

            fileReader = new GameObject("CustomMissText_FileReader").AddComponent<EntryFileReader>();
            BeatSaberMarkupLanguage.Util.MainMenuAwaiter.MainMenuInitializing += MainMenuInit;
        }

        public void MainMenuInit()
        {
            BSMLSettings.Instance.AddSettingsMenu("CustomMissText", "CustomMissText.UI.settingsMenu.bsml", SettingsMenu.instance);
        }

        [OnEnable]
        public void OnEnable()
        {
            PatchState(Config.Enabled);
        }

        [OnDisable]
        public void OnDisable()
        {
            BSMLSettings.Instance.RemoveSettingsMenu(SettingsMenu.instance);
            Object.Destroy(fileReader);
            PatchState(false);
        }

        public static void PatchState(bool state)
        {
            if (state)
                harmony?.PatchAll(Assembly.GetExecutingAssembly());
            else
                harmony?.UnpatchSelf();
        }
    }
}
