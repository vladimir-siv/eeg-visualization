using System.Collections.Generic;
using SharpGL;

namespace XEngine.Core
{
	public abstract class Scene
	{
		private static Dictionary<string, Scene> SceneCache = new Dictionary<string, Scene>();
		public static Scene Resolve(string sceneId) => SceneCache[sceneId];

		public Scene(string sceneId) => SceneCache.Add(sceneId, this);

		public virtual void Init(OpenGLControl control, float width, float height) { }
		public virtual void Draw(OpenGLControl control) { }
		public virtual void Exit(OpenGLControl control) { }
	}
}
