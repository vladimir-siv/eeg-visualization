﻿using GlmNet;

namespace XEngine.Shapes
{
	using XEngine.Data;

	public class Square : GeometricShape
	{
		public Square() :
			this
			(
				-1.0f, +1.0f, +0.0f, +1.0f, +0.0f, +0.0f,
				-1.0f, -1.0f, +0.0f, +0.0f, +0.0f, +1.0f,
				+1.0f, -1.0f, +0.0f, +0.0f, +0.0f, +1.0f,
				+1.0f, +1.0f, +0.0f, +1.0f, +0.0f, +0.0f
			)
		{

		}

		public Square
		(
			float x1, float y1, float z1, float r1, float g1, float b1,
			float x2, float y2, float z2, float r2, float g2, float b2,
			float x3, float y3, float z3, float r3, float g3, float b3,
			float x4, float y4, float z4, float r4, float g4, float b4
		) :
			this
			(
				new vec3(x1, y1, z1), new vec3(r1, g1, b1),
				new vec3(x2, y2, z2), new vec3(r2, g2, b2),
				new vec3(x3, y3, z3), new vec3(r3, g3, b3),
				new vec3(x4, y4, z4), new vec3(r4, g4, b4)
			)
		{

		}

		public Square
		(
			vec3 p1, vec3 c1,
			vec3 p2, vec3 c2,
			vec3 p3, vec3 c3,
			vec3 p4, vec3 c4
		) :
			this
			(
				new vertex[]
				{
					new vertex(p1, c1),
					new vertex(p2, c2),
					new vertex(p3, c3),
					new vertex(p4, c4),
				},
				new ushort[]
				{
					0, 1, 2,
					3, 0, 2
				}
			)
		{

		}

		private Square(vertex[] vertices, ushort[] indices = null) : base(new ShapeData(vertices, indices))
		{

		}
	}
}
