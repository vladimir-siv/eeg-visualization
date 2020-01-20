using System;
using System.Linq;
using System.Reflection;

namespace XEngine
{
	public static class XEngineActivator
	{
		public static void InitEngine()
		{
			foreach (var type in typeof(XEngineActivator).Assembly.GetTypes().Where(t => t.IsClass))
			{
				var engActivateAttr = type.GetCustomAttributes(typeof(XEngineActivationAttribute), false);
				if (engActivateAttr.Length == 0) continue;
				var attr = (XEngineActivationAttribute)engActivateAttr[0];
				type.GetMethod(attr.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
			}
		}
	}

	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class XEngineActivationAttribute : Attribute
	{
		public string MethodName { get; }
		public XEngineActivationAttribute(string methodName) => MethodName = methodName;
	}
}
