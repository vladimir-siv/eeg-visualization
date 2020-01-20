using System;
using System.Windows.Forms;
using XEngine;

namespace EEGVisualization
{
	public static class Program
	{
		[STAThread] static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			XEngineActivator.InitEngine();
			Application.Run(new MainForm());
		}
	}
}
