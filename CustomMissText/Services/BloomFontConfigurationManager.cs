using System;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEngine;

namespace CustomMissText.Services
{
    /// <summary>
    /// Code lazily taken from HitScoreVisualizer by Eris
    /// TO BE FAIR, PART OF THIS CODE WAS WRITTEN BY ME
    /// </summary>
    internal class BloomFontConfigurationManager : IDisposable
    {
        private readonly PluginConfig Config = Plugin.Config;
        private readonly Lazy<TMP_FontAsset> cachedTekoFont;
        private readonly Lazy<TMP_FontAsset> bloomTekoFont;

        public BloomFontConfigurationManager()
        {
            cachedTekoFont = new Lazy<TMP_FontAsset>(() => Resources.FindObjectsOfTypeAll<TMP_FontAsset>().First(x =>
            x.name.Contains("Teko-Medium SDF")), LazyThreadSafetyMode.ExecutionAndPublication);

            bloomTekoFont = new Lazy<TMP_FontAsset>(() =>
            {
                var distanceFieldShader = Resources.FindObjectsOfTypeAll<Shader>().First(x => x.name.Contains("TextMeshPro/Distance Field"));
                var bloomTekoFont = TMP_FontAsset.CreateFontAsset(cachedTekoFont.Value.sourceFontFile);
                bloomTekoFont.name = "Teko-Medium SDF (Bloom)";
                bloomTekoFont.material.shader = distanceFieldShader;

                return bloomTekoFont;
            }, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public void ConfigureFont(ref TextMeshPro text)
        {
            text.font = Config.UseBloomFont ? bloomTekoFont.Value : cachedTekoFont.Value;
        }

        public void Dispose()
        {
            if (bloomTekoFont.IsValueCreated)
            {
                UnityEngine.Object.Destroy(bloomTekoFont.Value);
            }
        }
    }
}
