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
					new vertex(new vec3(-.85f, -.20f, -1.0f), new vec3(+1.0f, +0.0f, +0.0f)),
					new vertex(new vec3(-.95f, -.20f, -1.0f), new vec3(+1.0f, +0.0f, +0.0f)),

					new vertex(new vec3(-.85f, -.35f, -1.0f), new vec3(+1.0f, +1.0f, +0.0f)),
					new vertex(new vec3(-.95f, -.35f, -1.0f), new vec3(+1.0f, +1.0f, +0.0f)),

					new vertex(new vec3(-.85f, -.50f, -1.0f), new vec3(+0.0f, +1.0f, +0.0f)),
					new vertex(new vec3(-.95f, -.50f, -1.0f), new vec3(+0.0f, +1.0f, +0.0f)),

					new vertex(new vec3(-.85f, -.65f, -1.0f), new vec3(+0.0f, +1.0f, +1.0f)),
					new vertex(new vec3(-.95f, -.65f, -1.0f), new vec3(+0.0f, +1.0f, +1.0f)),

					new vertex(new vec3(-.85f, -.80f, -1.0f), new vec3(+0.0f, +0.0f, +1.0f)),
					new vertex(new vec3(-.95f, -.80f, -1.0f), new vec3(+0.0f, +0.0f, +1.0f)),

					new vertex(new vec3(-.85f, -.95f, -1.0f), new vec3(+1.0f, +0.0f, +1.0f)),
					new vertex(new vec3(-.95f, -.95f, -1.0f), new vec3(+1.0f, +0.0f, +1.0f)),
				},
				new ushort[]
				{
					0,  1,  3,
					0,  3,  2,

					2,  3,  5,
					2,  5,  4,

					4,  5,  7,
					4,  7,  6,

					6,  7,  9,
					6,  9,  8,

					8,  9, 11,
					8, 11, 10,
				}
			)
		)
		{

		}
	}
}
