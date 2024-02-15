namespace ReRenderingOptions
{

    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using ReRenderingOptions.Systems;
    using Game.Tools;
    using UnityEngine.Rendering.HighDefinition;
    using UnityEngine.Rendering;
    using ReRenderingOptions;
    using System;
    using System.IO;
    using Lumina;
    using MonoMod.RuntimeDetour;
    using UnityEngine;

    public sealed class Mod : IMod
    {
        /// <summary>
        /// Mod properties.
        /// </summary>
        public const string ModName = "ReRenderingOptions";                    
        public static Mod Instance { get; private set; }
        internal ILog Log { get; private set; }
        public void OnLoad()
        {
            Instance = this;
            Log = LogManager.GetLogger(ModName);
            Log.Info("setting logging level to Debug");
            Log.effectivenessLevel = Level.Debug;
            Log.Info("loading");
        }



        /// <summary>
        /// Gets the mod's active settings configuration.
        /// </summary>
        internal ModSettings ActiveSettings { get; private set; }

        /// <summary>
        /// Called by the game when the game world is created. 
        /// </summary>
        /// <param name="updateSystem">Game update system.</param>
        public void OnCreateWorld(UpdateSystem updateSystem)
        {


            Console.WriteLine("Current value: " + ModSettings.DynamicResolutionCache);
            ActiveSettings = new(this);
            ActiveSettings.RegisterInOptionsUI();
            Localization.LoadTranslations(ActiveSettings, Log);
            updateSystem.UpdateAfter<ModeSystem>(SystemUpdatePhase.GameSimulation);// Update system.
            updateSystem.UpdateAfter<EnablerSystem>(SystemUpdatePhase.CompleteRendering);// Update system.
            string localLowDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            localLowDirectory = Path.Combine(localLowDirectory, "..", "LocalLow");
            string assemblyDirectory = Path.Combine(localLowDirectory, "Colossal Order", "Cities Skylines II", "Mods", "ReRenderingOptions");
            string settingsFilePath = Path.Combine(assemblyDirectory, "RROSettings.xml");

            Console.WriteLine("ReRenderingOptions 1.3", ConsoleColor.Blue, ConsoleColor.Blue);
            Console.WriteLine("Support: https://discord.gg/5gZgRNm29e", ConsoleColor.Blue, ConsoleColor.Blue);
            Console.WriteLine("Donate: https://shorturl.at/hmpCW", ConsoleColor.Blue, ConsoleColor.Blue);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" ────────────────────────────────── ");
            Console.ResetColor();
            UnityEngine.Debug.Log("ReRenderingOptions exported settings located at:" + settingsFilePath.ToString()); // Lets the user know where to find the file.
            ActiveSettings.Load(); // Loads the settings to the settings menu from an XML file.
            

        }
        /// <summary>
        /// Called by the game when the mod is disposed of.
        /// </summary>
        public void OnDispose()
        {
            Log.Info("disposing");
            Instance = null;
        }
    }
}