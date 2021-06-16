using CustomMissText.Installers;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Loader;
using IPA.Utilities;
using IPALogger = IPA.Logging.Logger;
using UnityEngine;
using SiraUtil.Zenject;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [Init] public Plugin(IPALogger iLogger, Config iConfig, PluginMetadata metadata, Zenjector zenjector)
        {
            defDir = Path.Combine(UnityGame.UserDataPath, "CustomMissText");
            defText = $@"{defDir}\Default.txt";

            config = iConfig.Generated<PluginConfig>();
            config.Version = metadata.Version;
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

        #region Tools (For use in the REPL shipped with RUE)
        /// <summary>
        /// Outputs all <typeparamref name="T"/>'s to the log matching the specified <paramref name="filter"/>.
        /// To search unfiltered, <paramref name="filter"/> must be <see langword="null"/> or whitespace.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        public static void OutputTypeObjectsToLog<T>(string filter) where T : Object
        {
            T[] objects = Resources.FindObjectsOfTypeAll<T>();

            if (string.IsNullOrWhiteSpace(filter))
            {
                int i = 0;
                foreach (T obj in objects)
                {
                    logger.Info($"[{i}] ({typeof(T)}) {obj.name}");
                    i++;
                }
            }
            else
            {
                IEnumerable<T> filteredObjs = objects.Where(x => x.name.Contains(filter));
                int i = 0;

                foreach (T obj in filteredObjs)
                {
                    logger.Info($"[{i}] ({typeof(T)}) {obj.name}");
                    i++;
                }
            }
        }
        #endregion
    }
}
