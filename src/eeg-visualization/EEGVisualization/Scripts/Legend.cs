using GlmNet;
using XEngine.Data;
using XEngine.Shapes;

namespace EEGVisualization.Scripts
{
	public class Legend : GeometricShape
	{
		public Legend() : base
		(
			new ShapeData
			(
				new vertex[]
				{
					new vertex(new vec3(-.95f, -.20f, -1.0f), new vec3(+1.0f, +0.0f, +0.0f)),
					new vertex(new vec3(-.95f, -.35f, -1.0f), new vec3(+1.0f, +1.0f, +0.0f)),
					new vertex(new vec3(-.85f, -.35f, -1.0f), new vec3(+1.0f, +1.0f, +0.0f)),
					new vertex(new vec3(-.85f, -.20f, -1.0f), new vec3(+1.0f, +0.0f, +0.0f)),

					new vertex(new vec3(-.95f, -.35f, -1.0f), new vec3(+1.0f, +1.0f, +0.0f)),
					new vertex(new vec3(-.95f, -.50f, -1.0f), new vec3(+0.0f, +1.0f, +0.0f)),
					new vertex(new vec3(-.85f, -.50f, -1.0f), new vec3(+0.0f, +1.0f, +0.0f)),
					new vertex(new vec3(-.85f, -.35f, -1.0f), new vec3(+1.0f, +1.0f, +0.0f)),

					new vertex(new vec3(-.95f, -.50f, -1.0f), new vec3(+0.0f, +1.0f, +0.0f)),
					new vertex(new vec3(-.95f, -.65f, -1.0f), new vec3(+0.0f, +1.0f, +1.0f)),
					new vertex(new vec3(-.85f, -.65f, -1.0f), new vec3(+0.0f, +1.0f, +1.0f)),
					new vertex(new vec3(-.85f, -.50f, -1.0f), new vec3(+0.0f, +1.0f, +0.0f)),

					new vertex(new vec3(-.95f, -.65f, -1.0f), new vec3(+0.0f, +1.0f, +1.0f)),
					new vertex(new vec3(-.95f, -.80f, -1.0f), new vec3(+0.0f, +0.0f, +1.0f)),
					new vertex(new vec3(-.85f, -.80f, -1.0f), new vec3(+0.0f, +0.0f, +1.0f)),
					new vertex(new vec3(-.85f, -.65f, -1.0f), new vec3(+0.0f, +1.0f, +1.0f)),

					new vertex(new vec3(-.95f, -.80f, -1.0f), new vec3(+0.0f, +0.0f, +1.0f)),
					new vertex(new vec3(-.95f, -.95f, -1.0f), new vec3(+1.0f, +0.0f, +1.0f)),
					new vertex(new vec3(-.85f, -.95f, -1.0f), new vec3(+1.0f, +0.0f, +1.0f)),
					new vertex(new vec3(-.85f, -.80f, -1.0f), new vec3(+0.0f, +0.0f, +1.0f)),
				},
				new ushort[]
				{
					 0,  1,  2,
					 3,  0,  2,

					 4,  5,  6,
					 7,  4,  6,

					 8,  9,  10,
					11,  8,  10,

					12, 13, 14,
					15, 12, 14,

					16, 17, 18,
					19, 16, 18,
				}
			)
		)
		{

		}
	}
}
