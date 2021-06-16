using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using SiraUtil.Converters;
using UnityEngine;

namespace CustomMissText
{
    public class PluginConfig
    {
        [NonNullable, UseConverter(typeof(VersionConverter))] public virtual SemVer.Version Version { get; set; }

        public virtual bool Enabled { get; set; } = true;
        [UseConverter(typeof(HexColorConverter))] public virtual Color DefaultTextColor { get; set; } = Color.red;
        public virtual bool Italics { get; set; } = true;
        public virtual string SelectedConfig { get; set; } = "Default";
    }
}
