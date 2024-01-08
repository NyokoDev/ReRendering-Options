using Game.Settings;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReRenderingOptions.Patches
{
    internal class LevelOfDetailPatch
    {
        [HarmonyPatch(typeof(LevelOfDetailQualitySettings))]
        [HarmonyPatch("Apply")]
        class LevelOfDetailQualitySettings_Apply_Patch
        {
            static bool Prefix()
            {
                UnityEngine.Debug.Log("ReRenderingOptions: Patched ingame settings with RRO settings");
                return false;
            }


        }
    }
}

