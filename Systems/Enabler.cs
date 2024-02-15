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
using Lumina;
using MonoMod.RuntimeDetour;
// The entirety of this code stands as the creation of Nyoko. Any attempt to redistribute or share this code without explicit authorization is strictly prohibited.
namespace ReRenderingOptions.Systems
{
    public partial class EnablerSystem : SystemBase
    {

        public bool executed = false;
        protected override void OnUpdate()
        {
            if(!executed)
            ModeSystem.Initialize();
            executed = true;

        }
    }
}

        




// The entirety of this code stands as the creation of Nyoko. Any attempt to redistribute or share this code without explicit authorization is strictly prohibited.