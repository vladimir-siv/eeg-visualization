using System;
using System.Windows.Input;

using GlmNet;
using SharpGL;

using XEngine;
using XEngine.Core;
using XEngine.Shapes;
using XEngine.Models;

using Cursor = System.Windows.Forms.Cursor;
using Control = System.Windows.Forms.Control;
using MouseButtons = System.Windows.Forms.MouseButtons;

namespace EEGVisualization.Scripts
{
	using EEG;

	[GenerateScene("EEGVisualization.MainScene")]
	public class MainScene : Scene
	{
		#region Shader Program Properties

		private uint ShaderProgramId { get; set; }

		private int ProjectMatrixLocation { get; set; }
		private int ViewMatrixLocation { get; set; }
		private int TranslateMatrixLocation { get; set; }
		private int ScaleMatrixLocation { get; set; }
		private int RotateMatrixLocation { get; set; }

		private int AmbientLightColorLocation { get; set; }
		private int AmbientLightPowerLocation { get; set; }

		private int LightSourcePositionLocation { get; set; }
		private int LightSourceColorLocation { get; set; }
		private int LightSourcePowerLocation { get; set; }

		private int EyePositionLocation { get; set; }

		private int ElectrodePositionsLocation { get; set; }
		private int ElectrodeCountLocation { get; set; }
		private int ElectrodeIndicesLocation { get; set; }
		private int ElectrodesUsedCountLocation { get; set; }
		private int ElectrodeValuesLocation { get; set; }
		private int ElectrodeMaxDistanceLocation { get; set; }

		private uint[] ArrayIds { get; } = new uint[2];
		private uint[] ModelBufferIds { get; } = new uint[2];
		private uint[] UIBufferIds { get; } = new uint[2];

		#endregion

		private GeometricShape Shape { get; set; }
		private GeometricShape UI { get; set; }

		private vec3 AmbientLightColor { get; } = new vec3(1.0f, 1.0f, 1.0f);
		private float AmbientLightPower { get; } = 0.25f;
		
		private vec3 LightSourcePosition { get; } = new vec3(-15.0f, 40.0f, 30.0f);
		private vec3 LightSourceColor { get; } = new vec3(1.0f, 1.0f, 1.0f);
		private float LightSourcePower { get; } = 60.0f; // Watts for instance

		private float ElectrodeMaxDistance { get; set; } = 50.0f;

		private Camera MainCamera { get; } = new Camera(new vec3(-30.0f, 20.0f, 30.0f));

		private vec2 ShapeRotationAngle { get; set; } = new vec2(0.0f, 0.0f);
		private float ShapeScale { get; set; } = 1.0f;
		private float Scroll { get; set; } = 0.0f;
		private vec2 LastMousePosition { get; set; } = new vec2(Cursor.Position.X, Cursor.Position.Y);
		private bool LastKeyLeft { get; set; } = false;
		private bool LastKeyRight { get; set; } = false;

		private void LoadSceneData(OpenGL gl)
		{
			var modelLoader = Model.Load("male_head");
			modelLoader.Wait();
			Shape = modelLoader.Result;
			UI = new Legend();

			gl.Enable(OpenGL.GL_DEPTH_TEST);
			gl.Enable(OpenGL.GL_CULL_FACE);

			gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

			gl.GenVertexArrays(ArrayIds.Length, ArrayIds);

			// Model data
			gl.BindVertexArray(ArrayIds[0]);
			gl.GenBuffers(ModelBufferIds.Length, ModelBufferIds);

			gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, ModelBufferIds[0]);
			gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, Shape.Indices, OpenGL.GL_STATIC_DRAW);

			gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, ModelBufferIds[1]);
			gl.BufferData(OpenGL.GL_ARRAY_BUFFER, Shape.Data, OpenGL.GL_STATIC_DRAW);

			gl.EnableVertexAttribArray(0);
			gl.EnableVertexAttribArray(1);
			gl.EnableVertexAttribArray(2);
			gl.VertexAttribPointer(0, Shape.GetAttribSize(0), Shape.GetAttribType(0), Shape.ShouldAttribNormalize(0), Shape.GetAttribStride(0), Shape.GetAttribOffset(0));
			gl.VertexAttribPointer(1, Shape.GetAttribSize(1), Shape.GetAttribType(1), Shape.ShouldAttribNormalize(1), Shape.GetAttribStride(1), Shape.GetAttribOffset(1));
			gl.VertexAttribPointer(2, Shape.GetAttribSize(2), Shape.GetAttribType(2), Shape.ShouldAttribNormalize(2), Shape.GetAttribStride(2), Shape.GetAttribOffset(2));

			// UI data
			gl.BindVertexArray(ArrayIds[1]);
			gl.GenBuffers(UIBufferIds.Length, UIBufferIds);

			gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, UIBufferIds[0]);
			gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, UI.Indices, OpenGL.GL_STATIC_DRAW);

			gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, UIBufferIds[1]);
			gl.BufferData(OpenGL.GL_ARRAY_BUFFER, UI.Data, OpenGL.GL_STATIC_DRAW);

			gl.EnableVertexAttribArray(0);
			gl.EnableVertexAttribArray(1);
			gl.EnableVertexAttribArray(2);
			gl.VertexAttribPointer(0, UI.GetAttribSize(0), UI.GetAttribType(0), UI.ShouldAttribNormalize(0), UI.GetAttribStride(0), UI.GetAttribOffset(0));
			gl.VertexAttribPointer(1, UI.GetAttribSize(1), UI.GetAttribType(1), UI.ShouldAttribNormalize(1), UI.GetAttribStride(1), UI.GetAttribOffset(1));
			gl.VertexAttribPointer(2, UI.GetAttribSize(2), UI.GetAttribType(2), UI.ShouldAttribNormalize(2), UI.GetAttribStride(2), UI.GetAttribOffset(2));
		}

		private void BuildShaders(OpenGL gl)
		{
			string GetCompileError(uint shaderId)
			{
				var compileStatus = new int[1];
				gl.GetShader(shaderId, OpenGL.GL_COMPILE_STATUS, compileStatus);
				if (compileStatus[0] != OpenGL.GL_TRUE)
				{
					var infoLogLength = new int[1];
					gl.GetShader(shaderId, OpenGL.GL_INFO_LOG_LENGTH, infoLogLength);
					var buffer = new System.Text.StringBuilder(infoLogLength[0]);
					gl.GetShaderInfoLog(shaderId, infoLogLength[0], IntPtr.Zero, buffer);
					return buffer.ToString();
				}

				return null;
			}
			string GetLinkError(uint progId)
			{
				var linkStatus = new int[1];
				gl.GetProgram(progId, OpenGL.GL_LINK_STATUS, linkStatus);
				if (linkStatus[0] != OpenGL.GL_TRUE)
				{
					var infoLogLength = new int[1];
					gl.GetProgram(progId, OpenGL.GL_INFO_LOG_LENGTH, infoLogLength);
					var buffer = new System.Text.StringBuilder(infoLogLength[0]);
					gl.GetProgramInfoLog(progId, infoLogLength[0], IntPtr.Zero, buffer);
					return buffer.ToString();
				}

				return null;
			}

			ShaderProgramId = gl.CreateProgram();

			var vertexShader = Controller.LoadShader("vertex");
			var fragmentShader = Controller.LoadShader("fragment");

			uint vertexShaderId = gl.CreateShader(OpenGL.GL_VERTEX_SHADER);
			uint fragmentShaderId = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);

			gl.ShaderSource(vertexShaderId, vertexShader);
			gl.ShaderSource(fragmentShaderId, fragmentShader);

			gl.CompileShader(vertexShaderId);
			var vertexCompileError = GetCompileError(vertexShaderId);
			if (vertexCompileError != null)
			{
				System.Windows.Forms.MessageBox.Show(vertexCompileError, "Vertex Compile Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}

			gl.CompileShader(fragmentShaderId);
			var fragmentCompileError = GetCompileError(fragmentShaderId);
			if (fragmentCompileError != null)
			{
				System.Windows.Forms.MessageBox.Show(fragmentCompileError, "Fragment Compile Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}

			gl.AttachShader(ShaderProgramId, vertexShaderId);
			gl.AttachShader(ShaderProgramId, fragmentShaderId);

			gl.LinkProgram(ShaderProgramId);
			var linkError = GetLinkError(ShaderProgramId);
			if (linkError != null)
			{
				System.Windows.Forms.MessageBox.Show(linkError, "Link Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
				return;
			}

			gl.DeleteShader(vertexShaderId);
			gl.DeleteShader(fragmentShaderId);

			gl.UseProgram(ShaderProgramId);

			ProjectMatrixLocation = gl.GetUniformLocation(ShaderProgramId, "project");
			ViewMatrixLocation = gl.GetUniformLocation(ShaderProgramId, "view");
			TranslateMatrixLocation = gl.GetUniformLocation(ShaderProgramId, "translate");
			ScaleMatrixLocation = gl.GetUniformLocation(ShaderProgramId, "scale");
			RotateMatrixLocation = gl.GetUniformLocation(ShaderProgramId, "rotate");
			
			AmbientLightColorLocation = gl.GetUniformLocation(ShaderProgramId, "ambient_light_color");
			AmbientLightPowerLocation = gl.GetUniformLocation(ShaderProgramId, "ambient_light_power");
			
			LightSourcePositionLocation = gl.GetUniformLocation(ShaderProgramId, "light_source_position");
			LightSourceColorLocation = gl.GetUniformLocation(ShaderProgramId, "light_source_color");
			LightSourcePowerLocation = gl.GetUniformLocation(ShaderProgramId, "light_source_power");
			
			EyePositionLocation = gl.GetUniformLocation(ShaderProgramId, "eye_position");

			ElectrodePositionsLocation = gl.GetUniformLocation(ShaderProgramId, "electrode_positions");
			ElectrodeCountLocation = gl.GetUniformLocation(ShaderProgramId, "electrode_count");
			ElectrodeIndicesLocation = gl.GetUniformLocation(ShaderProgramId, "electrode_indices");
			ElectrodesUsedCountLocation = gl.GetUniformLocation(ShaderProgramId, "electrodes_used_count");
			ElectrodeValuesLocation = gl.GetUniformLocation(ShaderProgramId, "electrode_values");
			ElectrodeMaxDistanceLocation = gl.GetUniformLocation(ShaderProgramId, "electrode_max_distance");
		}

		private void LoadEEGData(OpenGL gl)
		{
			Electrodes.Load().Wait();
			Voltages.Init().Wait();
		}

		public override void Init(OpenGLControl control, float width, float height)
		{
			control.MouseWheel += (s, e) => Scroll += e.Delta / System.Windows.Forms.SystemInformation.MouseWheelScrollDelta;
			LoadSceneData(control.OpenGL);
			BuildShaders(control.OpenGL);
			LoadEEGData(control.OpenGL);
		}

		private void Update(OpenGLControl control)
		{
			if (!Host.CurrentApplicationIsActive) return;

			if (control.ClientRectangle.Contains(control.PointToClient(Cursor.Position)))
			{
				var delta = new vec2(Cursor.Position.X, Cursor.Position.Y) - LastMousePosition;

				// Rotate camera
				if (Control.MouseButtons.HasFlag(MouseButtons.Middle))
				{
					MainCamera.Rotate(delta);
				}

				// Rotate shape
				if (Control.MouseButtons.HasFlag(MouseButtons.Left))
				{
					ShapeRotationAngle += new vec2(delta.y, delta.x);
				}
			}

			// Move camera
			var moveDelta = new vec3(0.0f, 0.0f, 0.0f);

			if (Keyboard.IsKeyDown(Key.W)) moveDelta.z -= 1.0f;
			if (Keyboard.IsKeyDown(Key.S)) moveDelta.z += 1.0f;
			if (Keyboard.IsKeyDown(Key.A)) moveDelta.x -= 1.0f;
			if (Keyboard.IsKeyDown(Key.D)) moveDelta.x += 1.0f;
			if (Control.MouseButtons.HasFlag(MouseButtons.XButton1)) moveDelta.y -= 1.0f;
			if (Control.MouseButtons.HasFlag(MouseButtons.XButton2)) moveDelta.y += 1.0f;

			ShapeScale += Scroll / 10.0f;
			Scroll = 0.0f;

			MainCamera.Move(moveDelta);

			// Arrows
			var keyLeft = Keyboard.IsKeyDown(Key.Left);
			var keyRight = Keyboard.IsKeyDown(Key.Right);
			if (!LastKeyLeft && keyLeft) Voltages.Back();
			if (!LastKeyRight && keyRight) Voltages.Forward();
			if (Keyboard.IsKeyDown(Key.Up)) ElectrodeMaxDistance += 0.25f;
			if (Keyboard.IsKeyDown(Key.Down)) ElectrodeMaxDistance -= 0.25f;
			LastKeyLeft = keyLeft;
			LastKeyRight = keyRight;
		}

		private void LateUpdate(OpenGLControl control)
		{
			LastMousePosition = new vec2(Cursor.Position.X, Cursor.Position.Y);
		}

		public override void Draw(OpenGLControl control)
		{
			var gl = control.OpenGL;

			gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
			gl.Viewport(0, 0, control.Width, control.Height);

			Update(control);
			LateUpdate(control);

			// Render Model
			gl.BindVertexArray(ArrayIds[0]);

			var project = glm.perspective(+60.0f * (float)Math.PI / 180.0f, (float)control.Width / (float)control.Height, +0.1f, +100.0f);
			var view = MainCamera.WorldToView;
			var translate = glm.translate(mat4.identity(), new vec3(+0.0f, +0.0f, +0.0f));
			var scale = glm.scale(mat4.identity(), new vec3(ShapeScale, ShapeScale, ShapeScale));
			var rotate = glm.rotate
			(
				glm.rotate
				(
					ShapeRotationAngle.y * (float)Math.PI / 180.0f, new vec3(+0.0f, +1.0f, +0.0f)
				),
					ShapeRotationAngle.x * (float)Math.PI / 180.0f, new vec3(+1.0f, +0.0f, +0.0f)
			);
			
			gl.UniformMatrix4(ProjectMatrixLocation, 1, false, project.to_array());
			gl.UniformMatrix4(ViewMatrixLocation, 1, false, view.to_array());
			gl.UniformMatrix4(TranslateMatrixLocation, 1, false, translate.to_array());
			gl.UniformMatrix4(ScaleMatrixLocation, 1, false, scale.to_array());
			gl.UniformMatrix4(RotateMatrixLocation, 1, false, rotate.to_array());

			gl.Uniform3(AmbientLightColorLocation, AmbientLightColor.x, AmbientLightColor.y, AmbientLightColor.z);
			gl.Uniform1(AmbientLightPowerLocation, AmbientLightPower);
			gl.Uniform3(LightSourcePositionLocation, LightSourcePosition.x, LightSourcePosition.y, LightSourcePosition.z);
			gl.Uniform3(LightSourceColorLocation, LightSourceColor.x, LightSourceColor.y, LightSourceColor.z);
			gl.Uniform1(LightSourcePowerLocation, LightSourcePower);
			gl.Uniform3(EyePositionLocation, MainCamera.Position.x, MainCamera.Position.y, MainCamera.Position.z);

			gl.Uniform1(ElectrodePositionsLocation, Electrodes.Count * 3, Electrodes.Data);
			gl.Uniform1(ElectrodeCountLocation, (uint)Electrodes.Count);
			gl.Uniform1(ElectrodeIndicesLocation, Voltages.ElectrodeIndices.Length, Voltages.ElectrodeIndices);
			gl.Uniform1(ElectrodesUsedCountLocation, (uint)Voltages.ElectrodeIndices.Length);
			gl.Uniform1(ElectrodeValuesLocation, Voltages.Values.Length, Voltages.Values);
			gl.Uniform1(ElectrodeMaxDistanceLocation, ElectrodeMaxDistance);

			gl.DrawElements(Shape.OpenGLShapeType, Shape.Indices.Length, OpenGL.GL_UNSIGNED_SHORT, IntPtr.Zero);

			// Render UI
			gl.BindVertexArray(ArrayIds[1]);

			var identity = mat4.identity();

			gl.UniformMatrix4(ProjectMatrixLocation, 1, false, identity.to_array());
			gl.UniformMatrix4(ViewMatrixLocation, 1, false, identity.to_array());
			gl.UniformMatrix4(TranslateMatrixLocation, 1, false, identity.to_array());
			gl.UniformMatrix4(ScaleMatrixLocation, 1, false, identity.to_array());
			gl.UniformMatrix4(RotateMatrixLocation, 1, false, identity.to_array());

			gl.Uniform3(AmbientLightColorLocation, 1.0f, 1.0f, 1.0f);
			gl.Uniform1(AmbientLightPowerLocation, 1.0f);
			gl.Uniform3(LightSourcePositionLocation, LightSourcePosition.x, LightSourcePosition.y, LightSourcePosition.z);
			gl.Uniform3(LightSourceColorLocation, 0.0f, 0.0f, 0.0f);
			gl.Uniform1(LightSourcePowerLocation, 0.0f);
			gl.Uniform3(EyePositionLocation, MainCamera.Position.x, MainCamera.Position.y, MainCamera.Position.z);

			gl.DrawElements(UI.OpenGLShapeType, UI.Indices.Length, OpenGL.GL_UNSIGNED_SHORT, IntPtr.Zero);
		}

		public override void Exit(OpenGLControl control)
		{
			var gl = control.OpenGL;
			gl.DeleteBuffers(ModelBufferIds.Length, ModelBufferIds);
			gl.DeleteBuffers(UIBufferIds.Length, UIBufferIds);
			gl.DeleteVertexArrays(ArrayIds.Length, ArrayIds);
			gl.UseProgram(0);
			gl.DeleteProgram(ShaderProgramId);
		}
	}
}
