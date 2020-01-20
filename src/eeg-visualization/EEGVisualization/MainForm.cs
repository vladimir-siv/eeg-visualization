using System;
using System.Windows.Forms;

using SharpGL;
using XEngine.Core;

namespace EEGVisualization
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void GLControl_OpenGLInitialized(object sender, EventArgs e)
		{
			SceneManager.CurrentScene.Init((OpenGLControl)sender, Width, Height);
		}

		private void GLControl_OpenGLDraw(object sender, RenderEventArgs args)
		{
			SceneManager.CurrentScene.Draw((OpenGLControl)sender);
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			SceneManager.CurrentScene.Exit(GLControl);
		}
	}
}
