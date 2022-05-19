using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly:InternalsVisibleTo("IPA.Config.Generated")]
namespace CustomMissText
{
    internal class PluginConfig
    {
        public virtual bool Enabled { get; set; } = true;
        public virtual bool Italics { get; set; } = true;
        public virtual bool UseBloomFont { get; set; } = false;
        public virtual string CurrentConfig { get; set; } = "Default";
        [UseConverter(typeof(HexColorConverter))] public virtual Color DefaultTextColor { get; set; } = Color.white;
    }
}
