using System;
using SharpGL;

namespace XEngine.Shapes
{
	using XEngine.Data;

	public class GeometricShape
	{
		protected ShapeData ShapeData { get; private set; }

		public vertex[] Vertices => ShapeData.Vertices;
		public ushort[] Indices => ShapeData.Indices;
		public float[] Data => ShapeData.Data;

		internal GeometricShape(ShapeData shapeData)
		{
			ShapeData = shapeData;
		}
		public virtual uint OpenGLShapeType => OpenGL.GL_TRIANGLES;

		public virtual int GetAttribSize(uint index)
		{
			return (int)vertex.AttribSize;
		}
		public virtual uint GetAttribType(uint index)
		{
			return OpenGL.GL_FLOAT;
		}
		public virtual bool ShouldAttribNormalize(uint index)
		{
			return false;
		}
		public virtual int GetAttribStride(uint index)
		{
			return (int)vertex.ByteSize;
		}
		public virtual IntPtr GetAttribOffset(uint index)
		{
			return new IntPtr(index * vertex.ByteSize / vertex.AttribSize);
		}
	}
}
