using GlmNet;

namespace XEngine.Core
{
	public sealed class Camera
	{
		private readonly vec3 IdentY = new vec3(0.0f, 1.0f, 0.0f);
		private vec3 StrafeDirection = new vec3(1.0f, 0.0f, 0.0f);

		public vec3 Position { get; private set; } = new vec3(0.0f, 0.0f, 0.0f);
		public vec3 ViewDirection { get; private set; } = new vec3(0.0f, 0.0f, -1.0f);

		public mat4 WorldToView => glm.lookAt(Position, Position + ViewDirection, IdentY);

		public float RotationSpeed { get; set; } = 0.005f;
		public float MovementSpeed { get; set; } = 1.0f;

		public Camera() : this(new vec3(0.0f, 0.0f, 0.0f)) { }
		public Camera(vec3 initialPosition)
		{
			MoveAt(initialPosition);
			LookAt(new vec3(0.0f, 0.0f, 0.0f));
		}

		public void Rotate(vec2 delta)
		{
			StrafeDirection = glm.cross(ViewDirection, IdentY);
			var rotx = glm.rotate(-delta.x * RotationSpeed, IdentY);
			var roty = glm.rotate(-delta.y * RotationSpeed, StrafeDirection);
			ViewDirection = (rotx * roty).to_mat3() * ViewDirection;
		}

		public void Move(vec3 delta)
		{
			Position += MovementSpeed * StrafeDirection * delta.x;
			Position += MovementSpeed * IdentY * delta.y;
			Position += MovementSpeed * ViewDirection * -delta.z;
		}

		public void LookAt(vec3 point)
		{
			ViewDirection = glm.normalize(-1f * Position + point);
			StrafeDirection = glm.cross(ViewDirection, IdentY);
		}

		public void MoveAt(vec3 position)
		{
			Position = position;
		}
	}
}
