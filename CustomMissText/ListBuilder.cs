using IPA.Utilities;
using SiraUtil.Tools;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using Zenject;

namespace CustomMissText
{
    internal class ListBuilder : IInitializable
    {
        PluginConfig _config;
        SiraLog _log;

        public ListBuilder(SiraLog log, PluginConfig config)
        {
            _log = log;
            _config = config;
        }

        public void Initialize()
        {
            SceneManager.activeSceneChanged += ActiveSceneChanged;
            DefaultFileChecker();
        }

        internal void ActiveSceneChanged(Scene scene, Scene _)
        {
            if (scene.name == "MenuEnvironment") ReadSelectedConfigFile();
        }

        internal void DefaultFileChecker()
        {
            if (Directory.Exists(Plugin.defDir) && File.Exists(Plugin.defText))
            {
                _log.Logger.Debug("Default directory and text file exist!");
                ReadSelectedConfigFile();
                return;
            }

            else
            {
                if (!Directory.Exists(Plugin.defDir))
                {
                    _log.Logger.Warn("Default directory does not exist. Making it now.");
                    Directory.CreateDirectory(Plugin.defDir);
                    _log.Logger.Notice("New directory made!");
                }

                if (!File.Exists(Plugin.defText))
                {
                    _log.Logger.Warn("Default text file does not exist. Making one now.");
                    using (FileStream fs = File.Create(Plugin.defText))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(_defaultConfig);
                        fs.Write(info, 0, info.Length);
                    }
                    _log.Logger.Notice("New text file made!");
                }

                ReadSelectedConfigFile();
            }
        }

        internal void ReadSelectedConfigFile()
        {
            Plugin.allEntries = null;

            try
            {
                Plugin.allEntries = ConfigReader(_config.SelectedConfig);
            }
            catch
            {
                _log.Logger.Error($"Config {_config.SelectedConfig} no longer exists, and was likely deleted.");
                _log.Logger.Error("Reverting to default and checking for default files.");

                _config.SelectedConfig = "Default";
                DefaultFileChecker();
            }
        }

        private List<string[]> ConfigReader(string configName)
        {
            List<string[]> entries = new List<string[]>();
            string path = $@"{UnityGame.UserDataPath}\CustomMissText\";

            var linesInFile = File.ReadLines(path + configName + ".txt", new UTF8Encoding(true));
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
                _log.Logger.Warn("Config found, but no entries were found. Reverting to default...");
                _config.SelectedConfig = "Default";
            }

            _log.Logger.Info($"Config {configName} contains {entries.Count} entries.");
            return entries;
        }

        internal const string _defaultConfig =
@"# CustomMissText v1.0.7
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
