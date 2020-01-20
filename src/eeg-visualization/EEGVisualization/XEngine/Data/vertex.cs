using GlmNet;

namespace XEngine.Data
{
	public struct vertex
	{
		public const uint AttribSize = 3u;
		public const uint AttribCount = 3u;

		public const uint Size = AttribSize * AttribCount;
		public const uint ByteSize = Size * sizeof(float);

		public vec3 position { get; set; }
		public vec3 color { get; set; }
		public vec3 normal { get; set; }

		public vertex(vec3 position, vec3 color) : this(position, color, new vec3(+0.0f, +0.0f, +1.0f)) { }
		public vertex(vec3 position, vec3 color, vec3 normal)
		{
			this.position = position;
			this.color = color;
			this.normal = normal;
		}

		public override string ToString()
		{
			return
				"[" +
					position.x +
					"," +
					position.y +
					"," +
					position.z +
					":" +
					color.x +
					"," +
					color.y +
					"," +
					color.z +
					":" +
					normal.x +
					"," +
					normal.y +
					"," +
					normal.z +
				"]";
		}
	}
}
