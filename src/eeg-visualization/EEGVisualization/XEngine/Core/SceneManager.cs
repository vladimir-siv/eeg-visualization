using System;
using System.Linq;

namespace XEngine.Core
{
	public static class SceneManager
	{
		public static Scene CurrentScene { get; private set; } = null;

		static SceneManager()
		{
			var sceneType = typeof(Scene);

			foreach (var type in sceneType.Assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && sceneType.IsAssignableFrom(t)))
			{
				var genSceneAttr = type.GetCustomAttributes(typeof(GenerateSceneAttribute), false);
				if (genSceneAttr.Length == 0) continue;
				var attr = (GenerateSceneAttribute)genSceneAttr[0];
				var scene = (Scene)Activator.CreateInstance(type);
				scene.SceneId = attr.SceneId;
				Scene.SceneCache.Add(scene.SceneId, scene);
			}
		}

		public static void LoadScene(string sceneId)
		{
			CurrentScene = Scene.Resolve(sceneId);
		}
	}

	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class GenerateSceneAttribute : Attribute
	{
		public string SceneId { get; }
		public GenerateSceneAttribute(string sceneId) => SceneId = sceneId;
	}
}
