using CustomMissText.Installers;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Loader;
using IPA.Utilities;
using IPALogger = IPA.Logging.Logger;
using UnityEngine.SceneManagement;
using SemVer;
using SiraUtil.Zenject;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CustomMissText
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        public static List<string[]> allEntries = null;
        public static string defDir;
        public static string defText;

        internal static Harmony _harmonyID = null;
        internal static IPALogger logger { get; private set; }
        internal static PluginConfig config;

        [Init] public Plugin(IPALogger iLogger, Config iConfig, PluginMetadata metadata, Zenjector zenjector)
        {
            defDir = Path.Combine(UnityGame.UserDataPath, "CustomMissText");
            defText = $@"{defDir}\Default.txt";

            config = iConfig.Generated<PluginConfig>();
            config.Version = metadata.Version;
            logger = iLogger;
            zenjector.OnApp<LoggerInstaller>().WithParameters(logger, config);

            bool dir = Directory.Exists(defDir);
            if (!dir) Directory.CreateDirectory(defDir);
        }

        [OnEnable] public void Enable()
        {
            if (_harmonyID is null) _harmonyID = new Harmony("bs.Exomanz.MissText");
            _harmonyID.PatchAll(Assembly.GetExecutingAssembly());

            SceneManager.activeSceneChanged += ActiveChanged;
            DefaultFileChecker();
        }

        [OnDisable]
        public void Disable()
        {
            _harmonyID.UnpatchAll(_harmonyID.Id);
            _harmonyID = null;

            SceneManager.activeSceneChanged -= ActiveChanged;
        }

        internal void ActiveChanged(Scene scene, Scene _) 
            { if (scene.name == "MenuEnvironment") ReadSelectedConfigFile(); }

        internal void DefaultFileChecker()
        {
            if (Directory.Exists(defDir) && File.Exists(defText))
            {
                logger.Debug("Default directory and text file exist!");
                ReadSelectedConfigFile();
                return;
            }

            else
            {
                if (!Directory.Exists(defDir))
                {
                    logger.Warn("Default directory does not exist. Making it now.");
                    Directory.CreateDirectory(defDir);
                    logger.Notice("New directory made!");
                }

                if (!File.Exists(defText))
                {
                    logger.Warn("Default text file does not exist. Making one now.");
                    using (FileStream fs = File.Create(defText))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(_defaultConfig);
                        fs.Write(info, 0, info.Length);
                    }
                    logger.Notice("New text file made!");
                }

                ReadSelectedConfigFile();
            }
        }

        public void ReadSelectedConfigFile()
        {
            try
            {
                allEntries = null;
                allEntries = ConfigReader(config.SelectedConfig);
            }
            catch
            {
                logger.Error($"Config '{config.SelectedConfig}' does not or no longer exists, and was likely deleted.");
                logger.Error("Switching to 'Default'");
                config.SelectedConfig = "Default";
                DefaultFileChecker();
            }
        }

        internal List<string[]> ConfigReader(string configName)
        {
            List<string[]> entries = new List<string[]>();
            string pathWithConfig = Path.Combine(defDir, configName + ".txt");

            var linesInFile = File.ReadLines(pathWithConfig, new UTF8Encoding(true));
            linesInFile = linesInFile.Where(x => x == "" || x[0] != '#');

            List<string> currentEntry = new List<string>();
            foreach (string line in linesInFile)
            {
                if (line == "")
                {
                    entries.Add(currentEntry.ToArray());
                    currentEntry.Clear();
                }
                else currentEntry.Add(line);
            }
            if (currentEntry.Count != 0) entries.Add(currentEntry.ToArray());
            if (currentEntry.Count == 0)
            {
                logger.Warn("Config found, but no entries were found! Reverting to default...");
                config.SelectedConfig = "Default";
            }

            logger.Info($"Config {configName} contains {entries.Count} entries");
            return entries;
        }

        internal const string _defaultConfig =
@"# CustomMissText v1.1.0
# by Exomanz and Arti
# 
# Use # for comments!
# Separate entries with empty lines; a random one will be picked each time the menu loads.
HECK

F

OwO

HITN'T

OOF

MS.

115
<size=50%>just kidding</size>

HISS

NOPE

WHOOSH

OOPS

C-C-C-COMBO
BREAKER

I MEANT TO
DO THAT

MOSS

MASS

MESS

MUSS

MYTH

KISS

# The following lines suggest by @E8 on BSMG
MISSCLICK

HIT OR MISS

LAG

TRACKING

LUL";
    }
}
