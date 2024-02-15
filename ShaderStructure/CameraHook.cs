using System;
using Game;
using Lumina;
using ReRenderingOptions;
using ReRenderingOptions.Settings;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;


/*
 * The ShaderStructure class encapsulates the structure of shaders used in this project. The complete code for ShaderStructure has been sourced from the Dynamic Resolution project, available at:
 * https://github.com/d235j/Skylines-DynamicResolution/tree/cc7d04df204b74c1ba781dc3a5f492ba30ce6b61
 * This class plays a crucial role in defining the structure and behavior of shaders within our Lumina project. By incorporating the Dynamic Resolution code, we leverage proven implementations and practices, fostering code reuse and maintaining consistency with the Dynamic Resolution project's shader-related functionality. Any modifications or enhancements to the ShaderStructure class should be carefully synchronized with the corresponding code in the Dynamic Resolution repository to ensure compatibility and benefit from ongoing improvements made by the Dynamic Resolution project.
 */


namespace Lumina
{
    public class CameraHook : MonoBehaviour
    {

        public static CameraHook instance = null;




#if DEBUG
    public static Rect mainCameraRect;
    public static Rect undergroundCameraRect;
#endif

        private RenderTexture rt;


        ModSettings ModSettings;
        public static float userSSAAFactor = 1f;
        public static float currentSSAAFactor { get; set; }

        public float sliderMaximum = 3.0f;

        private static bool initialized = false;

        public Rect cameraPixelRect;

        private GameObject dummyGameObject;
        private CameraRenderer cameraRenderer;

        public bool showConfigWindow = false;

#if DEBUG
    private Rect windowRect = new Rect(64, 64, 350, 230);
#else
        private Rect windowRect = new Rect(64, 64, 350, 170);
#endif




        public CameraController cameraController;


        private Texture2D bgTexture;


        private float dtAccum = 0.0f;
        private int frameCount = 0;
        private float fps = 0.0f;

        void OnDestroy()
        {
            GetComponent<Camera>().enabled = true;
            Destroy(dummyGameObject);
        }

        public void Awake()
        {
            instance = this;

            currentSSAAFactor = userSSAAFactor = GlobalVariables.DynamicResolution;


            cameraController = FindObjectOfType<CameraController>();


            bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, Color.grey);
            bgTexture.Apply();
            Initialize();
            UnityEngine.Debug.Log("CALLED AWAKE AT CAMERAHOOK");
        }



        public void SetInGameAA(bool state)
        {
            var camera = GetComponent<Camera>();

            if (!state)
            {
                // Disable AA if enabled
                var hdCamera = camera.GetComponent<HDAdditionalCameraData>();
                if (hdCamera != null)
                {
                    hdCamera.antialiasing = HDAdditionalCameraData.AntialiasingMode.None;
                }
            }
            else
            {
                // Enable SMAA if not already enabled
                var hdCamera = camera.GetComponent<HDAdditionalCameraData>();
                if (hdCamera != null && hdCamera.antialiasing != HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing)
                {
                    hdCamera.antialiasing = HDAdditionalCameraData.AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                }
            }
        }


        public int width
        {
            get { return (int)(Screen.width * currentSSAAFactor); }
        }
        public int height
        {
            get { return (int)(Screen.height * currentSSAAFactor); }
        }

        public int internalWidth
        {
            get { return (int)(cameraPixelRect.width * currentSSAAFactor); }
        }
        public int internalHeight
        {
            get { return (int)(cameraPixelRect.height * currentSSAAFactor); }
        }

        public void SetSSAAFactor(float factor, bool lowerVRAMUsage)
        {
            var width = Screen.width * factor;
            var height = Screen.height * factor;

            Destroy(rt);
            rt = new RenderTexture((int)width, (int)height, 24, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);

            var hook = dummyGameObject.GetComponent<CameraRenderer>();
            hook.fullResRT = rt;

            if (hook.halfVerticalResRT != null)
            {
                Destroy(hook.halfVerticalResRT);
            }

            if (!lowerVRAMUsage)
            {
                hook.halfVerticalResRT = new RenderTexture(Screen.width, (int)height, 0);
            }
            else
            {
                hook.halfVerticalResRT = null;
            }

            Destroy(CameraRenderer.mainCamera.targetTexture);
            CameraRenderer.mainCamera.targetTexture = rt;

            currentSSAAFactor = factor;
            userSSAAFactor = factor;

            initialized = true;
        }

        public void Initialize()
        {
            var camera = gameObject.GetComponent<Camera>();
            if (camera == null)
            {
                // Add a Camera component if it doesn't exist
                camera = gameObject.AddComponent<Camera>();
                if (camera == null)
                {
                    Debug.LogError("Failed to add Camera component to the GameObject.");
                    return;
                }
            }

            cameraPixelRect = camera.pixelRect;
            camera.enabled = false;

            var width = Screen.width * userSSAAFactor;
            var height = Screen.height * userSSAAFactor;
            rt = new RenderTexture((int)width, (int)height, 24, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear);

            dummyGameObject = new GameObject();
            if (dummyGameObject == null)
            {
                Debug.LogError("Failed to create dummy GameObject.");
                return;
            }
            var dummy = dummyGameObject.AddComponent<Camera>();
            if (dummy == null)
            {
                Debug.LogError("Failed to add Camera component to dummy GameObject.");
                Destroy(dummyGameObject); // Clean up the created GameObject
                return;
            }
            dummy.cullingMask = 0;
            dummy.depth = -3;
            dummy.tag = "MainCamera";
            dummy.pixelRect = cameraPixelRect;

            cameraRenderer = dummyGameObject.AddComponent<CameraRenderer>();
            if (cameraRenderer == null)
            {
                Debug.LogError("Failed to add CameraRenderer component to dummy GameObject.");
                Destroy(dummyGameObject); // Clean up the created GameObject
                return;
            }
            cameraRenderer.fullResRT = rt;
            cameraRenderer.halfVerticalResRT = new RenderTexture(Screen.width, (int)height, 0);

            if (CameraRenderer.mainCamera == null)
            {
                Debug.LogError("MainCamera reference is null.");
                Destroy(dummyGameObject); // Clean up the created GameObject
                return;
            }

            CameraRenderer.mainCamera.targetTexture = null;
            CameraRenderer.mainCamera.pixelRect = cameraPixelRect;

            currentSSAAFactor = userSSAAFactor;
            initialized = true;
        }
    }
}

