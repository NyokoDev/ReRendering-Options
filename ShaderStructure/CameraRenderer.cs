using System;
using System.Reflection;
using UnityEngine;
using Game;
using System.Runtime.CompilerServices;
using System.IO;



/*
 * The ShaderStructure class encapsulates the structure of shaders used in this project. The complete code for ShaderStructure has been sourced from the Dynamic Resolution project, available at:
 * https://github.com/d235j/Skylines-DynamicResolution/tree/cc7d04df204b74c1ba781dc3a5f492ba30ce6b61
 * This class plays a crucial role in defining the structure and behavior of shaders within our Lumina project. By incorporating the Dynamic Resolution code, we leverage proven implementations and practices, fostering code reuse and maintaining consistency with the Dynamic Resolution project's shader-related functionality. Any modifications or enhancements to the ShaderStructure class should be carefully synchronized with the corresponding code in the Dynamic Resolution repository to ensure compatibility and benefit from ongoing improvements made by the Dynamic Resolution project.
 */

namespace Lumina
{
    
    public class CameraRenderer : MonoBehaviour
    {
        public RenderTexture fullResRT;
        public RenderTexture halfVerticalResRT;

        public static Camera mainCamera;
        public static Camera camera;

        private Rect unitRect;

        private Material downsampleShader;
        private Material downsampleX2Shader;

        private string checkErrorMessage = null;


        private static Camera undergroundCamera;

        private static FieldInfo undergroundRGBDField;

        private static CameraController cameraController;
        private static FieldInfo cachedFreeCameraField;

        private static string cachedModPath = null;

        CameraRenderer instance;


        static string modPath
        {
            get
            {
                if (cachedModPath == null)
                {
                    string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                    cachedModPath = Path.GetDirectoryName(assemblyLocation);
                }

                return cachedModPath;
            }
        }

        void HandleCheckError(string message)
        {
#if (DEBUG)
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, message);
#endif
            if (checkErrorMessage == null)
            {
                checkErrorMessage = message;
            }
            else
            {
                checkErrorMessage += "; " + message;
            }
        }

        void ThrowPendingCheckErrors()
        {
            if (checkErrorMessage != null)
            {
                throw new Exception(checkErrorMessage);
            }
        }

        void CheckAssetBundle(AssetBundle assetBundle, string assetsUri)
        {
            if (assetBundle == null)
            {
                HandleCheckError("AssetBundle with URI '" + assetsUri + "' could not be loaded");
            }
#if (DEBUG)
            else
            {
                DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "Mod Assets URI: " + assetsUri);
                foreach (string asset in assetBundle.GetAllAssetNames())
                {
                    DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "Asset: " + asset);
                }
            }
#endif
        }

        void CheckShader(Shader shader, string source)
        {
            if (shader == null)
            {
                HandleCheckError("Shader " + source + " is missing or invalid");
            }
            else
            {
                if (!shader.isSupported)
                {
                    HandleCheckError("Shader '" + shader.name + "' " + source + " is not supported");
                }
#if (DEBUG)
                else
                {
                    DebugOutputPanel.AddMessage(
                        PluginManager.MessageType.Message,
                        "Shader '" + shader.name + "' " + source + " loaded");
                }
#endif
            }
        }

        void CheckShader(Shader shader, AssetBundle assetBundle, string shaderAssetName)
        {
            CheckShader(shader, "from asset '" + shaderAssetName + "'");
        }

        void CheckMaterial(Material material, string materialAssetName)
        {
            if (material == null)
            {
                HandleCheckError("Material for shader '" + materialAssetName + "' could not be created");
            }
#if (DEBUG)
            else
            {
                DebugOutputPanel.AddMessage(
                    PluginManager.MessageType.Message,
                    "Material for shader '" + materialAssetName + "' created");
            }
#endif
        }

        void LoadShaders()
        {
            instance = this;
            Console.WriteLine("The mod path is: " + modPath);
            string assetsUri;
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                assetsUri = "file:///" + modPath.Replace("\\", "/") + "/dynamicresolutionshaders_windows";
            }
            else if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                assetsUri = "file:///" + modPath.Replace("\\", "/") + "/dynamicresolutionshaders_mac";
            }
            else if (Application.platform == RuntimePlatform.LinuxPlayer)
            {
                assetsUri = "file:///" + modPath.Replace("\\", "/") + "/dynamicresolutionshaders_linux";
            }
            else
            {
                throw new Exception("[LUMINA] Shader not found. Ensure the shader is located in the mod folder.");
            }
            WWW www = new WWW(assetsUri);
            AssetBundle assetBundle = www.assetBundle;

            CheckAssetBundle(assetBundle, assetsUri);
            ThrowPendingCheckErrors();

            string downsampleAssetName = "downsampleShader.shader";
            string downsampleX2AssetName = "downsampleX2Shader.shader";
            Shader downsampleShaderContent = assetBundle.LoadAsset(downsampleAssetName) as Shader;
            Shader downsampleX2ShaderContent = assetBundle.LoadAsset(downsampleX2AssetName) as Shader;

            CheckShader(downsampleShaderContent, assetBundle, downsampleAssetName);
            CheckShader(downsampleX2ShaderContent, assetBundle, downsampleX2AssetName);
            ThrowPendingCheckErrors();

            string downsampleShaderMaterialAsset = downsampleAssetName;
            string downsampleX2ShaderMaterialAsset = downsampleX2AssetName;
            downsampleShader = new Material(downsampleShaderContent);
            downsampleX2Shader = new Material(downsampleX2ShaderContent);

            CheckMaterial(downsampleShader, downsampleShaderMaterialAsset);
            CheckMaterial(downsampleX2Shader, downsampleX2ShaderMaterialAsset);
            ThrowPendingCheckErrors();

            assetBundle.Unload(false);

            UnityEngine.Debug.Log("Loaded shaders.");
        }

        public void Awake()
        {
            camera = GetComponent<Camera>();
            mainCamera = Camera.main;

            unitRect = new Rect(0f, 0f, 1f, 1f);

  
            LoadShaders();
            UnityEngine.Debug.Log("CALLED AWAKE AT CAMERARENDERER");




        }
    
     

        public static void Update()
        {
            camera.fieldOfView = mainCamera.fieldOfView;
            camera.nearClipPlane = mainCamera.nearClipPlane;
            camera.farClipPlane = mainCamera.farClipPlane;
            camera.transform.position = mainCamera.transform.position;
            camera.transform.rotation = mainCamera.transform.rotation;
            camera.rect = mainCamera.rect;
        }

     
      

        public void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            if (fullResRT == null)
            {
                return;
            }

            var oldRect = mainCamera.rect;
            mainCamera.rect = unitRect;
            mainCamera.targetTexture = fullResRT;
            mainCamera.Render();
            mainCamera.targetTexture = null;
            mainCamera.rect = oldRect;

#if DEBUG
            CameraHook.mainCameraRect = oldRect;
#endif

            float factor = 0.0001f;

            if (factor != 1.0f && halfVerticalResRT != null)
            {

                Material shader = downsampleShader;

                if (factor <= 2.0f)
                {
                    shader = downsampleX2Shader;
                }

                downsampleShader.SetVector("_ResampleOffset", new Vector4(fullResRT.texelSize.x, 0.0f, 0.0f, 0.0f));
                Graphics.Blit(fullResRT, halfVerticalResRT, shader);

                downsampleShader.SetVector("_ResampleOffset", new Vector4(0.0f, fullResRT.texelSize.y, 0.0f, 0.0f));
                Graphics.Blit(halfVerticalResRT, dst, shader);
            }
            else
            {
                Graphics.Blit(fullResRT, dst);
            }
        }

    }
}
