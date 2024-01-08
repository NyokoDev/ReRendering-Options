/*
This game modification, "Exporter.cs," is the property and creation of NYOKO. Permission is hereby granted, free of charge, to any person obtaining a copy
of this game modification and associated documentation files (the "game modification"), to use the
game modification for personal or non-commercial purposes only. The sale or commercial
use of the Software is strictly prohibited.
*/

using Colossal.IO.AssetDatabase;
using Game.Common;
using Game.PSI.Internal;
using Game.Rendering;
using Game.Simulation.Flow;
using ReRenderingOptions.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

namespace ReRenderingOptions.Exporter
{
    
public class SettingsExporter
    {
        public static void ExportSettings()
        {
            string assemblyDirectory = AppDomain.CurrentDomain.BaseDirectory;
            

            string settingsFilePath = Path.Combine(assemblyDirectory, "RROSettings.txt");

            try
            {
                using (StreamWriter writer = new StreamWriter(settingsFilePath))
                {
                    RenderingSystem renderingSystem = World.DefaultGameObjectInjectionWorld?.GetExistingSystemManaged<RenderingSystem>();
                    if (renderingSystem != null)
                    {
                        writer.WriteLine("RERENDERING OPTIONS");
                        writer.WriteLine();
                        writer.WriteLine("Rendering System Settings:");
                        writer.WriteLine($"Level of Detail: {renderingSystem.levelOfDetail}");
                        writer.WriteLine($"LOD Cross Fade: {renderingSystem.lodCrossFade}");
                        writer.WriteLine($"Max Light Count: {renderingSystem.maxLightCount}");
                        writer.WriteLine();
                    }

                    BatchMeshSystem batchMeshSystem = World.DefaultGameObjectInjectionWorld?.GetExistingSystemManaged<BatchMeshSystem>();
                    if (batchMeshSystem != null)
                    {
                        writer.WriteLine("Batch Mesh System Settings:");
                        writer.WriteLine($"Memory Budget: {(long)batchMeshSystem.memoryBudget / 1048576} MB");
                        writer.WriteLine($"Strict Memory Budget: {batchMeshSystem.strictMemoryBudget}");
                        writer.WriteLine();
                    }

                    writer.WriteLine("Global Variables Settings:");
                    writer.WriteLine($"Global Quality Level: {GlobalVariables.GlobalQualityLevel}");
                    writer.WriteLine($"Global Texture Mipmap Limit: {GlobalVariables.globalTextureMipmapLimit}");
                    writer.WriteLine($"Shadow Distance: {GlobalVariables.shadowDistance}");
                    writer.WriteLine($"Shadow Cascades: {GlobalVariables.shadowCascades}");
                    writer.WriteLine($"Shadow Near Plane Offset: {GlobalVariables.shadowNearPlaneOffset}");
                    writer.WriteLine($"Realtime Reflection Probes: {GlobalVariables.realtimeReflectionProbes}");
                    writer.WriteLine($"Billboards Face Camera Position: {GlobalVariables.billboardsFaceCameraPosition}");
                    writer.WriteLine($"Async Upload Time Slice: {GlobalVariables.asyncUploadTimeSlice}");
                    writer.WriteLine($"Async Upload Buffer Size: {GlobalVariables.asyncUploadBufferSize}");
                    writer.WriteLine($"Terrain Detail Density Scale: {GlobalVariables.terrainDetailDensityScale}");
                    writer.WriteLine($"Terrain Pixel Error: {GlobalVariables.terrainPixelError}");
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exporting settings: {ex.Message}");
            }
        }
    }

}

