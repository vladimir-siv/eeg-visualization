using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace XEngine
{
	public static class Host
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

		public static bool CurrentApplicationIsActive
		{
			get
			{
				var activatedHandle = GetForegroundWindow();
				if (activatedHandle == IntPtr.Zero) return false;

				var procId = Process.GetCurrentProcess().Id;
				int activeProcId;
				GetWindowThreadProcessId(activatedHandle, out activeProcId);

				return activeProcId == procId;
			}
		}
	}
}
