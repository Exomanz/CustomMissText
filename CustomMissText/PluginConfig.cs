using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UnityEngine;

namespace CustomMissText
{
    public class PluginConfig
    {
        public virtual bool Enabled { get; set; } = true;
        [UseConverter(typeof(HexColorConverter))] public virtual Color DefaultTextColor { get; set; } = Color.white;
        public virtual bool Italics { get; set; } = true;
        public virtual string SelectedConfig { get; set; } = "Default";
    }
}
