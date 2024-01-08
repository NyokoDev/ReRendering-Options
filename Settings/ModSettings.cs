// <copyright file="ModSettings.cs" company="algernon (K. Algernon A. Sheppard)">
// Copyright (c) algernon (K. Algernon A. Sheppard). All rights reserved.
// Licensed under the Apache Licence, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// See LICENSE.txt file in the project root for full license information.
// </copyright>

namespace ReRenderingOptions
{
    using System.Xml.Serialization;
    using Colossal.IO.AssetDatabase;
    using ReRenderingOptions;
    using ReRenderingOptions.Settings;
    using Game.Modding;
    using Game.Settings;
    using UnityEngine;
    using System;
    using Game.UI.Widgets;
    using System.Collections.Generic;

    using UnityEngine.Scripting;




    /// <summary>
    /// The mod's settings.
    /// </summary>
    [FileLocation(Mod.ModName)]
    public class ModSettings : ModSetting
    {
        private bool ForceLowGlobalQualityValue;
        public float LowestValueSet = 0.001f;
        public float HighestValueSet = 5f;
        public static float Fraction = 1f;
     
 
   
        /// <summary>
        /// Initializes a new instance of the <see cref="ModSettings"/> class.
        /// </summary>  
        /// <param name="mod"><see cref="IMod"/> instance.</param>
        public ModSettings(IMod mod)
            : base(mod)
        {
        }

        /// <summary>
        /// Basic low global quality setting.
        /// </summary>
        [SettingsUISection("ForceLowGlobalQuality")]
        public bool ForceLowGlobalQuality
        {
            get => ForceLowGlobalQualityValue;

            set
            {
                ForceLowGlobalQualityValue = value;

                // Clear conflicting settings.
                if (value)
                {

                    GlobalVariables.GlobalQualityLevel = 0;
                    
                }

                // Ensure state.
                EnsureState();
            }
        }

    
        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
        public float GlobalQuality { get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
        public float globalTextureMipmapLimit { get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
        public float ShadowDistance { get; set; }


        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
        public float ShadowCascades { get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
        public float shadowNearPlaneOffset { get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        public bool realtimeReflectionProbes{ get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        public bool billboardsFaceCameraPosition { get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
        public float asyncUploadTimeSlice { get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
        public float asyncUploadBufferSize { get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
        public float terrainDetailDensityScale { get; set; }

        [SettingsUIAdvanced]
        [SettingsUISection("Advanced")]
        [SettingsUISlider(min = 0f, max = 100f, step = 1f, unit = "percentage", scaleDragVolume = true, scalarMultiplier = 100f)]
        public float terrainPixelError { get; set; }





        public override void Apply()
        {
            base.Apply();
            GlobalVariables.GlobalQualityLevel = Convert.ToInt32(GlobalQuality);
            GlobalVariables.globalTextureMipmapLimit = Convert.ToInt32(globalTextureMipmapLimit);
            GlobalVariables.shadowDistance = Convert.ToInt32(ShadowDistance);
            GlobalVariables.shadowCascades = Convert.ToInt32(ShadowCascades);
            GlobalVariables.shadowNearPlaneOffset = Convert.ToInt32(shadowNearPlaneOffset);
            GlobalVariables.realtimeReflectionProbes = realtimeReflectionProbes;
            GlobalVariables.billboardsFaceCameraPosition = billboardsFaceCameraPosition;
            GlobalVariables.asyncUploadTimeSlice = Convert.ToInt32(asyncUploadTimeSlice);
            GlobalVariables.asyncUploadBufferSize = Convert.ToInt32(asyncUploadBufferSize);
            GlobalVariables.terrainDetailDensityScale = Convert.ToInt32(terrainDetailDensityScale);
            GlobalVariables.terrainPixelError = Convert.ToInt32(terrainPixelError);



          


        }
     

        /// <summary>
        /// Sets a value indicating whether the mod's settings should be reset.
        /// </summary>
        [XmlIgnore]
        [SettingsUIButton]
        [SettingsUISection("ResetModSettings")]
        [SettingsUIConfirmation]
        public bool ResetModSettings
        {
            set
            {
                // Apply defaults.
                SetDefaults();

                // Save.
                ApplyAndSave();
            }
        }

        /// <summary>
        /// Restores mod settings to default.
        /// </summary>
        public override void SetDefaults()
        {
            ForceLowGlobalQualityValue = false;
            GlobalVariables.GlobalQualityLevel = 1;
            GlobalVariables.globalTextureMipmapLimit = 1;
            GlobalVariables.shadowDistance = 1;
            GlobalVariables.shadowCascades = 1;
            GlobalVariables.shadowNearPlaneOffset = 1;
            GlobalVariables.realtimeReflectionProbes = true;
            GlobalVariables.billboardsFaceCameraPosition = true;
            GlobalVariables.asyncUploadTimeSlice = 1;
            GlobalVariables.asyncUploadBufferSize = 1;
            GlobalVariables.terrainDetailDensityScale = 1;
            GlobalVariables.terrainPixelError = 1;


        }

        /// <summary>
        /// Enables Unlock All as the default option and that no options are duplicated.
        /// </summary>
        private void EnsureState()
        {
         
        }

     

        public class CacheValues
        {
            public static float CacheQualityLevel;
        }
    }
}