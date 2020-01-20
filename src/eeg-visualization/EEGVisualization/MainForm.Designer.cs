namespace EEGVisualization
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.GLControl = new SharpGL.OpenGLControl();
			((System.ComponentModel.ISupportInitialize)(this.GLControl)).BeginInit();
			this.SuspendLayout();
			// 
			// GLControl
			// 
			this.GLControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GLControl.DrawFPS = false;
			this.GLControl.FrameRate = 60;
			this.GLControl.Location = new System.Drawing.Point(0, 0);
			this.GLControl.Name = "GLControl";
			this.GLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL4_4;
			this.GLControl.RenderContextType = SharpGL.RenderContextType.NativeWindow;
			this.GLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
			this.GLControl.Size = new System.Drawing.Size(800, 600);
			this.GLControl.TabIndex = 0;
			this.GLControl.OpenGLInitialized += new System.EventHandler(this.GLControl_OpenGLInitialized);
			this.GLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.GLControl_OpenGLDraw);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 600);
			this.Controls.Add(this.GLControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.Text = "EEG Visualization";
			((System.ComponentModel.ISupportInitialize)(this.GLControl)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private SharpGL.OpenGLControl GLControl;
	}
}

