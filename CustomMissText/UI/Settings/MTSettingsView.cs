using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using IPA.Utilities;
using SiraUtil.Tools;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace CustomMissText.UI.Settings
{
    [ViewDefinition("CustomMissText.UI.Settings.Views.main.bsml")]
    //[HotReload(RelativePathToLayout = @"..\Settings\Views\main.bsml")]
    internal class MTSettingsView : BSMLAutomaticViewController
    {
        ListBuilder _builder;
        PluginConfig _config;
        SiraLog _log;

        [Inject]
        public void Construct(ListBuilder builder, PluginConfig config, SiraLog log)
        {
            _builder = builder;
            _config = config;
            _log = log;
        }

        [UIValue("ConfigList")] public List<object> configList = ConfigLister();
#pragma warning disable CS0649
        [UIComponent("DDLSConfigs")] public DropDownListSetting _ddlsConfigs;
        [UIParams] BSMLParserParams _parserParams;
#pragma warning restore CS0649

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            RefreshConfigList();
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
            _builder.ReadSelectedConfigFile();
        }

        internal static List<object> ConfigLister()
        {
            List<object> foundConfigs = new List<object>();
            var configsAvailable = Directory.GetFiles($@"{UnityGame.UserDataPath}\CustomMissText\", "*.txt");

            foreach (string config in configsAvailable)
                foundConfigs.Add(config.Replace($@"{UnityGame.UserDataPath}\CustomMissText\", "").Replace(".txt", ""));

            return foundConfigs;
        }

        [UIAction("RefreshConfigList")]
        public void RefreshConfigList()
        {
            _ddlsConfigs.values = ConfigLister();
            _ddlsConfigs.UpdateChoices();
            _parserParams.EmitEvent("EmitRefreshConfigList");
        }

        protected bool EnablePlugin
        {
            get => _config.Enabled;
            set
            {
                _config.Enabled = value;

                if (_config.Enabled)
                {
                    _log.Logger.Debug("Patching...");
                    Plugin._harmonyID.PatchAll(Assembly.GetExecutingAssembly());
                }
                else
                {
                    _log.Logger.Debug("Unpatching...");
                    Plugin._harmonyID.UnpatchAll(Plugin._harmonyID.Id);
                }
            }
        }

        protected bool Italics
        {
            get => _config.Italics;
            set => _config.Italics = value;
        }

        protected Color DefaultTextColor
        {
            get => _config.DefaultTextColor;
            set => _config.DefaultTextColor = value;
        }

        protected string SelectedConfig
        {
            get => _config.SelectedConfig;
            set => _config.SelectedConfig = value;
        }
    }
}
