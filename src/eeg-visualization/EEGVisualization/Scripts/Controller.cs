using System.IO;

using XEngine;
using XEngine.Core;

namespace EEGVisualization.Scripts
{
	[XEngineActivation(nameof(Init))]
	public class Controller
	{
		private static string MainScene = "EEGVisualization.MainScene";

		private static void Init()
		{
			SceneManager.LoadScene(MainScene);
		}

		public static string LoadShader(string shaderName)
		{
			return ManifestResourceManager.LoadFile($"Shaders/{shaderName}.glsl");
		}

		public static Stream LoadFromResources(string resource)
		{
			return ManifestResourceManager.LoadResource($"Resources/{resource}");
		}
	}
}
