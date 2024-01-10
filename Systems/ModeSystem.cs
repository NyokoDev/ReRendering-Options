using Game.UI.InGame;
using Game.UI;
using System;
using System.Reflection;
using System.Reflection.Emit;
using Game.Rendering;
using Colossal.UI.Binding;
using Game.Tools;
using Game.Simulation;
using Unity.Entities;
using Game.UI.Widgets;
using System.Collections.Generic;
using Game;
using Game.Rendering.CinematicCamera;
using UnityEngine;


using Game.Rendering.Utilities;
using ReRenderingOptions.Settings;
// The entirety of this code stands as the creation of Nyoko. Any attempt to redistribute or share this code without explicit authorization is strictly prohibited.
namespace ReRenderingOptions.Systems
{
    public partial class ModeSystem : SystemBase
    {

        /// <summary>
        /// Update method.
        /// </summary>
        protected override void OnUpdate()
        {
            ApplyGraphicsSettings();

        }
   
        /// <summary>
        /// Applies graphics settings in-game. Kind of not needed since we already do that on settings menu.
        /// </summary>
        void ApplyGraphicsSettings()
        {
            QualitySettings.SetQualityLevel(GlobalVariables.GlobalQualityLevel, true); // Set to the lowest quality level
            QualitySettings.globalTextureMipmapLimit = GlobalVariables.globalTextureMipmapLimit; // Reduces texture quality to minimum
            QualitySettings.shadows = ShadowQuality.Disable; // Disable shadows
            QualitySettings.shadowResolution = ShadowResolution.Low; // Set shadow resolution to low
            QualitySettings.shadowDistance = GlobalVariables.shadowDistance; // Set shadow distance to 0
            QualitySettings.shadowCascades = GlobalVariables.shadowCascades; // Disable shadow cascades
            QualitySettings.shadowProjection = ShadowProjection.CloseFit; // Use close-fit shadow projection
            QualitySettings.shadowNearPlaneOffset = GlobalVariables.shadowNearPlaneOffset; // Set shadow near plane offset
            QualitySettings.realtimeReflectionProbes = GlobalVariables.realtimeReflectionProbes; // Disable realtime reflection probes
            QualitySettings.billboardsFaceCameraPosition = GlobalVariables.billboardsFaceCameraPosition; // Billboards don't face camera position
            QualitySettings.antiAliasing = 0; // Disable anti-aliasing
            QualitySettings.asyncUploadTimeSlice = GlobalVariables.asyncUploadTimeSlice; // Set async upload time slice
            QualitySettings.asyncUploadBufferSize = GlobalVariables.asyncUploadBufferSize; // Set async upload buffer size
            QualitySettings.terrainDetailDensityScale = GlobalVariables.terrainDetailDensityScale;
            QualitySettings.terrainPixelError = GlobalVariables.terrainPixelError;



        }
        
    }
}




// The entirety of this code stands as the creation of Nyoko. Any attempt to redistribute or share this code without explicit authorization is strictly prohibited.