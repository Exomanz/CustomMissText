using CustomMissText.Services;
using HarmonyLib;
using IPA.Utilities;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

namespace CustomMissText.Patches
{
    [HarmonyPatch(typeof(MissedNoteEffectSpawner))]
    internal class MissedNoteEffectSpanwer_HandleNoteWasMissed
    {
        private static MethodInfo info = null;
            
        public static bool inMethod = true;
        internal static DiContainer Container = null;

        private static FlyingTextSpawner _spawner;
        private static FlyingTextSpawner _spawnerBase
        {
            get
            {
                if (_spawner != null) return _spawner;
                else
                {
                    _spawner = new GameObject("CustomMissTextSpawner").AddComponent<FlyingTextSpawner>();

                    var installers = UnityEngine.Object.FindObjectsOfType<MonoInstallerBase>();
                    foreach (MonoInstallerBase installer in installers)
                    {
                        Container = installer.GetProperty<DiContainer, MonoInstallerBase>("Container");
                        if (Container != null && Container.HasBinding<FlyingTextEffect.Pool>())
                        {
                            Container.Inject(_spawner);
                            Container.Bind<BloomFontConfigurationManager>().ToSelf().AsSingle();
                            break;
                        }
                    }
                    _spawner.SetField("_color", Plugin.Config.DefaultTextColor);

                    return _spawner;
                }
            }
        }

        [HarmonyTargetMethod]
        internal static MethodBase TargetMethod()
        {
            if (info != null) return info;

            try
            {
                info = typeof(MissedNoteEffectSpawner).GetMethod("HandleNoteWasMissed");
                return info;

            }
            catch (Exception e)
            {
                Plugin.Logger.Error(e);
                return default;
            }
        }

        [HarmonyPrefix]
        internal static bool Prefix(NoteController noteController, float ____spawnPosZ)
        {
            if (_spawnerBase == null)
            {
                Plugin.Logger.Error("Failed to inject FlyingTextSpawner");
                return true;
            }

            NoteData noteData = noteController.noteData;
            if (noteData.colorType != ColorType.None)
            {
                Vector3 pos = noteController.noteTransform.position;
                Quaternion rot = noteController.worldRotation;
                pos = noteController.inverseWorldRotation * pos;
                pos.z = ____spawnPosZ;
                pos = rot * pos;
                inMethod = true;
                _spawner.SpawnText(pos, noteController.worldRotation, noteController.inverseWorldRotation, "MISS");
                inMethod = false;
            }

            return false;
        }
    }
}
