using HarmonyLib;
using IPA.Utilities;
using TMPro;
using UnityEngine;
using Zenject;

namespace CustomMissText.HarmonyPatches
{
    [HarmonyPatch(typeof(MissedNoteEffectSpawner), nameof(MissedNoteEffectSpawner.HandleNoteWasMissed), MethodType.Normal)]
    internal class MissedNoteEffectSpawner_HandleNoteWasMissed
    {
        public static bool inMethod = false;
        static FlyingTextSpawner _spawner;
        static FlyingTextSpawner _spawnerBase
        {
            get
            {
                if (_spawner != null) return _spawner;
                else
                {
                    _spawner = new GameObject("CustomMissTextSpawner").AddComponent<FlyingTextSpawner>();

                    var installers = Object.FindObjectsOfType<MonoInstallerBase>();
                    foreach (var installer in installers)
                    {
                        var container = installer.GetProperty<DiContainer, MonoInstallerBase>("Container");

                        if (container != null && container.HasBinding<FlyingTextEffect.Pool>())
                        {
                            container.Inject(_spawner);
                            break;
                        }
                    }
                    _spawner.SetField("_fontSize", 3f);
                    _spawner.SetField("_color", Color.red);

                    return _spawner;
                }
            }
        }

        [HarmonyPrefix]
        static bool Prefix(MissedNoteEffectSpawner __instance, NoteController noteController, float ____spawnPosZ)
        {
            if (_spawnerBase == null)
            {
                Plugin.logger.Error("Failed to inject FlyingTextSpawner!");
                return true;
            }

            NoteData noteData = noteController.noteData;
            if (noteData.colorType == ColorType.ColorA || noteData.colorType == ColorType.ColorB)
            {
                Vector3 vector = noteController.noteTransform.position;
                Quaternion worldRotation = noteController.worldRotation;
                vector = noteController.inverseWorldRotation * vector;
                vector.z = ____spawnPosZ;
                vector = worldRotation * vector;
                inMethod = true;
                _spawner.SpawnText(vector, noteController.worldRotation, noteController.inverseWorldRotation, "MISS");
                inMethod = false;
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(FlyingTextEffect), nameof(FlyingTextEffect.InitAndPresent))]
    public class FlyingTextEffect_InitAndPresent
    {
        [HarmonyPrefix]
        static bool Prefix(ref string text, ref TextMeshPro ____text)
        {
            if (MissedNoteEffectSpawner_HandleNoteWasMissed.inMethod && text == "MISS")
            {
                System.Random r = new System.Random();
                int entryPicked = r.Next(Plugin.allEntries.Count);

                text = string.Join("\n", Plugin.allEntries[entryPicked]);
                ____text.overflowMode = TextOverflowModes.Overflow;
                ____text.enableWordWrapping = false;
                ____text.richText = true;
            }
            return true;
        }
    }
}
