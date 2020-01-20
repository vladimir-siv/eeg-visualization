using System.Collections.Generic;
using SharpGL;

namespace XEngine.Core
{
	public abstract class Scene
	{
		internal static Dictionary<string, Scene> SceneCache = new Dictionary<string, Scene>();
		public static Scene Resolve(string sceneId) => SceneCache[sceneId];

		public string SceneId { get; internal set; } = string.Empty;

		public virtual void Init(OpenGLControl control, float width, float height) { }
		public virtual void Draw(OpenGLControl control) { }
		public virtual void Exit(OpenGLControl control) { }
	}
}
