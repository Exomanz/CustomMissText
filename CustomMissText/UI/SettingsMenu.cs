using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Util;
using CustomMissText.Services;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CustomMissText.UI
{
    public class SettingsMenu : NotifiableSingleton<SettingsMenu>
    {
        private PluginConfig Config => Plugin.Config;

        [UIValue("enabled")]
        public bool Enabled
        {
            get => Config.Enabled;
            set
            {
                Plugin.PatchState(value);
                Config.Enabled = value;
            }
        }

        [UIValue("use-italics")]
        public bool Italics
        {
            get => Config.Italics;
            set => Config.Italics = value;
        }

        [UIValue("use-bloom-font")]
        public bool UseBloomFont
        {
            get => Config.UseBloomFont;
            set => Config.UseBloomFont = value;
        }

        [UIValue("default-text-color")]
        public Color DefaultTextColor
        {
            get => Config.DefaultTextColor;
            set => Config.DefaultTextColor = value;
        }

        [UIValue("current-config")]
        public string CurrentConfig
        {
            get => Config.CurrentConfig;
            set
            {
                Config.CurrentConfig = value;
                EntryFileReader.Instance.ReadCurrentConfig();
            }
        }

        [UIValue("config-list")]
        public List<object> ConfigList = GetConfigList();

        private static List<object> GetConfigList()
        {
            List<object> configs = new List<object>();
            string[] totalConfigs = Directory.GetFiles(Helpers.DIRECTORY, "*.txt");

            foreach (string config in totalConfigs)
            {
                configs.Add(config.Replace(Helpers.DIRECTORY + @"\", "").Replace(".txt", ""));
            }

            return configs;
        }
    }
}
