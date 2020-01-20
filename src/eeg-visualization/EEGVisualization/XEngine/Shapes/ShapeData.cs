using System;

namespace XEngine.Shapes
{
	using XEngine.Data;

	public struct ShapeData
	{
		public vertex[] Vertices { get; private set; }
		public ushort[] Indices { get; private set; }

		public float[] Data
		{
			get
			{
				var dataLength = Indices.Length * vertex.Size;

				var data = new float[dataLength];

				for (int i = 0; i < Indices.Length; ++i)
				{
					var index = Indices[i];
					var vertex = Vertices[index];

					for (int c = 0; c < vertex.Size; ++c)
					{
						float value = 0.0f;

						switch (c)
						{
							case 0: value = vertex.position.x; break;
							case 1: value = vertex.position.y; break;
							case 2: value = vertex.position.z; break;
							case 3: value = vertex.color.x; break;
							case 4: value = vertex.color.y; break;
							case 5: value = vertex.color.z; break;
							case 6: value = vertex.normal.x; break;
							case 7: value = vertex.normal.y; break;
							case 8: value = vertex.normal.z; break;
							default: value = 0.0f; break;
						}

						data[index * vertex.Size + c] = value;
					}
				}

				return data;
			}
		}

		public ShapeData(vertex[] vertices, ushort[] indices = null)
		{
			Vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));

			if (indices == null)
			{
				indices = new ushort[Vertices.Length];

				for (ushort i = 0; i < indices.Length; ++i)
				{
					indices[i] = i;
				}
			}
			
			Indices = indices;
		}
	}
}
