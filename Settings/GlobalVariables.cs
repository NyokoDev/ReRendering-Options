using Game.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

namespace ReRenderingOptions.Settings
{
    internal class GlobalVariables
    {

        /// <summary>
        /// Quality properties to set.
        /// </summary>
        public static int GlobalQualityLevel;
        public static int globalTextureMipmapLimit;
        public static int shadowDistance;
        public static int shadowCascades;
        public static int shadowProjection;
        public static int shadowNearPlaneOffset;
        public static bool realtimeReflectionProbes;
        public static bool billboardsFaceCameraPosition;
        public static int antiAliasing;
        public static int asyncUploadTimeSlice;
        public static int asyncUploadBufferSize;
        public static int terrainDetailDensityScale;
        public static int terrainPixelError;
        public static float DynamicResolution;

        /// <summary>
        /// LevelofDetail field, returns 0 as default.
        /// </summary>
        public static float levelOfDetail
        {
            get;
            set;
        }
    }
}
        
   
