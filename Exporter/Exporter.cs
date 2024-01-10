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
using System.Xml;
using Unity.Entities;
using UnityEngine;

namespace ReRenderingOptions.Exporter
{

    public class SettingsExporter
    {
        public static void ExportSettings()
        {
            string assemblyDirectory = AppDomain.CurrentDomain.BaseDirectory;


            string settingsFilePath = Path.Combine(assemblyDirectory, "RROSettings2.txt");

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
        public static void SaveGlobalVariablesToXml(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlElement root = xmlDoc.CreateElement("GlobalVariables");
            xmlDoc.AppendChild(root);

            AddXmlElement(xmlDoc, root, "GlobalQualityLevel", GlobalVariables.GlobalQualityLevel.ToString("D"));
            AddXmlElement(xmlDoc, root, "globalTextureMipmapLimit", GlobalVariables.globalTextureMipmapLimit.ToString("D"));
            AddXmlElement(xmlDoc, root, "shadowDistance", GlobalVariables.shadowDistance.ToString());
            AddXmlElement(xmlDoc, root, "shadowCascades", GlobalVariables.shadowCascades.ToString());
            AddXmlElement(xmlDoc, root, "shadowNearPlaneOffset", GlobalVariables.shadowNearPlaneOffset.ToString());
            AddXmlElement(xmlDoc, root, "realtimeReflectionProbes", GlobalVariables.realtimeReflectionProbes.ToString());
            AddXmlElement(xmlDoc, root, "billboardsFaceCameraPosition", GlobalVariables.billboardsFaceCameraPosition.ToString());
            AddXmlElement(xmlDoc, root, "asyncUploadTimeSlice", GlobalVariables.asyncUploadTimeSlice.ToString("D"));
            AddXmlElement(xmlDoc, root, "asyncUploadBufferSize", GlobalVariables.asyncUploadBufferSize.ToString("D"));
            AddXmlElement(xmlDoc, root, "terrainDetailDensityScale", GlobalVariables.terrainDetailDensityScale.ToString("D"));
            AddXmlElement(xmlDoc, root, "terrainPixelError", GlobalVariables.terrainPixelError.ToString("D"));
            AddXmlElement(xmlDoc, root, "levelOfDetail", ((float)GlobalVariables.levelOfDetail).ToString());


            xmlDoc.Save(filePath);
        }

        private static void AddXmlElement(XmlDocument xmlDoc, XmlElement parentElement, string elementName, string value)
        {
            XmlElement xmlElement = xmlDoc.CreateElement(elementName);
            xmlElement.InnerText = value;
            parentElement.AppendChild(xmlElement);
        }

        public static void LoadGlobalVariablesFromXml(string filePath)
        {
            if (File.Exists(filePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);

                XmlNodeList nodes = xmlDoc.DocumentElement.ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    try
                    {


                        switch (node.Name)
                        {
                            case "GlobalQualityLevel":
                                GlobalVariables.GlobalQualityLevel = Convert.ToInt32(node.InnerText);
                                break;
                            case "globalTextureMipmapLimit":
                                GlobalVariables.globalTextureMipmapLimit = Convert.ToInt32(node.InnerText);
                                break;
                            case "shadowDistance":
                                GlobalVariables.shadowDistance = Convert.ToInt32(node.InnerText);
                                break;
                            case "shadowCascades":
                                GlobalVariables.shadowCascades = Convert.ToInt32(node.InnerText);
                                break;
                            case "shadowNearPlaneOffset":
                                GlobalVariables.shadowNearPlaneOffset = Convert.ToInt32(node.InnerText);
                                break;
                            case "realtimeReflectionProbes":
                                GlobalVariables.realtimeReflectionProbes = Convert.ToBoolean(node.InnerText);
                                break;
                            case "billboardsFaceCameraPosition":
                                GlobalVariables.billboardsFaceCameraPosition = Convert.ToBoolean(node.InnerText);
                                break;
                            case "asyncUploadTimeSlice":
                                GlobalVariables.asyncUploadTimeSlice = Convert.ToInt32(node.InnerText);
                                break;
                            case "asyncUploadBufferSize":
                                GlobalVariables.asyncUploadBufferSize = Convert.ToInt32(node.InnerText);
                                break;
                            case "terrainDetailDensityScale":
                                GlobalVariables.terrainDetailDensityScale = Convert.ToInt32(node.InnerText);
                                break;
                            case "terrainPixelError":
                                GlobalVariables.terrainPixelError = Convert.ToInt32(node.InnerText);
                                break;
                            case "levelOfDetail":
                                float parsedValue;
                                if (float.TryParse(node.InnerText, out parsedValue))
                                {
                                    GlobalVariables.levelOfDetail = parsedValue;
                                }
                                else
                                {
                                    // Handling for all fields not explicitly handled
                                    UnityEngine.Debug.Log("Couldn't master Level of Detail.");
                                }

                                break;
                            default:
                                // Handling for all fields not explicitly handled
                                string fieldValue = node.InnerText;
                                break;
                        }
                    }
                    catch (FormatException ex)
                    {
                        UnityEngine.Debug.Log($"Format Exception: {ex.Message} occurred while parsing '{node.Name}' field.");
                        // Handle the FormatException as needed
                    }
                }
            }
            else
            {
                Console.WriteLine("RRO: XML file not found or you haven't set any settings.");
            }
        }
    }
}




