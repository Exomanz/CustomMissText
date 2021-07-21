using CustomMissText.Installers;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Utilities;
using IPALogger = IPA.Logging.Logger;
using SiraUtil.Zenject;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CustomMissText
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static IPALogger logger { get; private set; }
        internal static PluginConfig config { get; set; }

        internal static Harmony _harmonyID = null;
        public static List<string[]> allEntries = null;
        public static string defDir;
        public static string defText;

        [Init] public Plugin(IPALogger iLogger, Config iConfig, Zenjector zenjector)
        {
            defDir = Path.Combine(UnityGame.UserDataPath, "CustomMissText");
            defText = $@"{defDir}\Default.txt";

            config = iConfig.Generated<PluginConfig>();
            logger = iLogger;

            zenjector.OnApp<AppInitInstaller>().WithParameters(logger, config);
            zenjector.OnMenu<MenuButtonUIInstaller>();

            bool dir = Directory.Exists(defDir);
            if (!dir) Directory.CreateDirectory(defDir);
        }

        [OnEnable] public void Enable()
        {
            if (_harmonyID is null) _harmonyID = new Harmony("bs.Exomanz.MissText");
            if (config.Enabled) _harmonyID.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnDisable]
        public void Disable()
        {
            _harmonyID.UnpatchAll(_harmonyID.Id);
            _harmonyID = null;
        }
    }
}
