using IPA.Config.Stores.Attributes;
using SiraUtil.Converters;
using UnityEngine;

namespace CustomMissText
{
    public class PluginConfig
    {
        [NonNullable, UseConverter(typeof(VersionConverter))] public virtual SemVer.Version Version { get; set; } = new SemVer.Version("0.0.0");

        public virtual bool Enabled { get; set; } = true;
        public virtual Color TextColor { get; set; } = Color.white;
        public virtual string SelectedConfig { get; set; } = "Default";
    }
}
