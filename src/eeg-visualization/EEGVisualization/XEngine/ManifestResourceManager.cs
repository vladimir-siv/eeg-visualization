using System.Reflection;
using System.IO;

namespace XEngine
{
	public static class ManifestResourceManager
	{
		public static string LoadFile(string fileName)
		{
			using (var stream = LoadResource(fileName))
			{
				using (var reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}

		public static Stream LoadResource(string resourceName)
		{
			var executingAssembly = Assembly.GetExecutingAssembly();
			var pathToDots = resourceName.Replace("\\", ".").Replace("/", ".");
			var location = string.Format("{0}.{1}", executingAssembly.GetName().Name, pathToDots);
			return executingAssembly.GetManifestResourceStream(location);
		}
	}
}
