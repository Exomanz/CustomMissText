using CustomMissText.Services;
using HarmonyLib;
using TMPro;
using Zenject;

namespace CustomMissText.Patches
{
    [HarmonyPatch(typeof(FlyingTextEffect), nameof(FlyingTextEffect.InitAndPresent), MethodType.Normal)]
    internal class FlyingTextEffect_InitAndPresent
    {
        private static DiContainer Container => MissedNoteEffectSpanwer_HandleNoteWasMissed.Container;
        private static EntryFileReader reader => EntryFileReader.Instance;

        [HarmonyPrefix]
        internal static bool Prefix(ref string text, ref TextMeshPro ____text)
        {
            if (MissedNoteEffectSpanwer_HandleNoteWasMissed.inMethod && text == "MISS")
            {
                System.Random rnd = new System.Random();
                int entryNum = rnd.Next((int)reader?.fileEntries?.Count);
                TextMeshPro tmp = ____text;

                text = string.Join("\n", reader?.fileEntries?[entryNum]);
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
