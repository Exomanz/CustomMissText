using Logger = IPA.Logging.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomMissText.Services
{
    public class EntryFileReader : MonoBehaviour
    {
        public static EntryFileReader Instance { get; private set; }

        public List<string[]> fileEntries = new List<string[]>();
        private Logger Logger => Plugin.Logger;
        private PluginConfig Config => Plugin.Config;

        public void Awake()
        {
            Instance = this;

            DontDestroyOnLoad(this);
            CheckDirectory();
            SceneManager.activeSceneChanged += CheckDirectoryAndReadFileOnMenu;
        }

        private async void CheckDirectory()
        {
            if (!Directory.Exists(Helpers.DIRECTORY))
            {
                Logger.Warn("CustomMissText Directory is not present; re-creating it and populating default file.");
                Directory.CreateDirectory(Helpers.DIRECTORY);
                await WriteDefaultConfigFile();
            }

            if (!File.Exists(Helpers.DEFAULT_FILE_PATH))
            {
                Logger.Warn("Default CustomMissText file is missing; re-creating it and populating default value.");
                await WriteDefaultConfigFile();
            }
        }

        private void CheckDirectoryAndReadFileOnMenu(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "MainMenu" && oldScene.name != "GameCore" && Config.Enabled)
            {
                Logger.Debug($"Reading entries from entry file '{Config.CurrentConfig}'");
                CheckDirectory();
                ReadCurrentConfig();
            }
        }

        private async Task WriteDefaultConfigFile()
        {
            try
            {
                using StreamWriter streamWriter = new StreamWriter(Helpers.DEFAULT_FILE_PATH, false);
                string data = Helpers.DEFAULT_CONFIG;
                await streamWriter.WriteAsync(data);
            }
            catch (Exception ex)
            {
                Logger.Error("Error occuring while writing default file!\n" + ex);
            }
        }
        
        public async void ReadCurrentConfig()
        {
            fileEntries.Clear();
            fileEntries = await ListBuilder(Config.CurrentConfig);
        }

        private async Task<List<string[]>> ListBuilder(string configName)
        {
            CheckDirectory();
            string pathToReadFrom = Path.Combine(Helpers.DIRECTORY, configName + ".txt");

            // Returned variable
            List<string[]> fileEntries = new List<string[]>();

            // Representation of an entry within the file.
            List<string> currentEntry = new List<string>();

            await Task.Factory.StartNew(delegate
            {
                IEnumerable<string> linesInFile = File.ReadLines(pathToReadFrom, new UTF8Encoding(true));

                // Remove Comments
                IEnumerable<string> filteredResults = linesInFile.Where(line => line == "" || line[0] != '#');

                // Iterates through the file.
                // 'currentEntry' resembles a (multi-line) entry and is converted to an array and added to 'fileEntries'.
                // This process is repeated for every line in the file.
                foreach (string line in filteredResults)
                {
                    if (line == "")
                    {
                        fileEntries.Add(currentEntry.ToArray());
                        currentEntry.Clear();
                    }
                    else currentEntry.Add(line);
                }

                // 'currentEntry' is usually cleared at the end of the iteration but we check again just in case.
                if (currentEntry.Count != 0) 
                    fileEntries.Add(currentEntry.ToArray());
            });

            if (fileEntries.Count == 0)
            {
                if (Config.CurrentConfig == "Default")
                {
                    Logger.Error("Default entry file is empty; something has gone terribly wrong.\nAttempting to repopulate the file.");
                    await WriteDefaultConfigFile();
                    return await ListBuilder(configName);
                }

                Logger.Warn("Config found, but contains no entries; reverting to default.");
                Config.CurrentConfig = "Default";
                return await ListBuilder(configName);
            }

            Logger.Debug($"Config '{configName}' contains {fileEntries.Count} entries.");
            return fileEntries;
        }
    }
}
