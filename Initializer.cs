using Colossal.Logging;
using Game.Tools;
using Game;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.Net;

namespace ReRenderingOptions
{
    static class Initializer
    {
        /// <summary>
        /// Pending implementation.
        /// </summary>
        public static void OnLoad()
        {

        }
        public static class ConsoleColors
        {
            public static void WriteColored(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            {
                Console.ForegroundColor = foregroundColor;
                Console.BackgroundColor = backgroundColor;
                Console.Write(message);
                Console.ResetColor();
            }

            public static void WriteLineColored(string message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            {
                Console.ForegroundColor = foregroundColor;
                Console.BackgroundColor = backgroundColor;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }


        public static void OnInitialized()
        {

        
        }

    }
}

