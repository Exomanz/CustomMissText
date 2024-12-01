using CustomMissText.Services;
using HarmonyLib;
using TMPro;
using Zenject;

namespace CustomMissText.Patches
{
    [HarmonyPatch(typeof(FlyingTextEffect), nameof(FlyingTextEffect.InitAndPresent), MethodType.Normal)]
    internal class FlyingTextEffect_InitAndPresent
    {
        private static DiContainer Container => MissedNoteEffectSpawner_HandleNoteWasMissed.Container;
        private static EntryFileReader reader => EntryFileReader.Instance;

        [HarmonyPrefix]
        internal static bool Prefix(ref string text, ref TextMeshPro ____text)
        {
            if (MissedNoteEffectSpawner_HandleNoteWasMissed.inMethod && text == "MISS")
            {
                var entries = reader?.fileEntries;
                if (entries == null) return true;
                System.Random rnd = new System.Random();
                int entryNum = rnd.Next(entries.Count);
                TextMeshPro tmp = ____text;

                text = string.Join("\n", entries[entryNum]);
                tmp.overflowMode = TextOverflowModes.Overflow;
                tmp.enableWordWrapping = false;
                tmp.richText = true;

                if (Plugin.Config.Italics) 
                    tmp.fontStyle = FontStyles.Italic;
                if (Plugin.Config.UseBloomFont)
                    Container.Resolve<BloomFontConfigurationManager>().ConfigureFont(ref tmp);
            }

            return true;
        }
    }
}
