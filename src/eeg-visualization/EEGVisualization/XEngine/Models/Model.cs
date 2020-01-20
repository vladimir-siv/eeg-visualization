using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

using GlmNet;

namespace XEngine.Models
{
	using XEngine;
	using XEngine.Data;
	using XEngine.Shapes;

    public static class Model
	{
		public static async Task<GeometricShape> Load(string model)
		{
			var positions = new List<vec3>(1024);
			var color = new vec3(232 / 255f, 176 / 255f, 141 / 255f);
			var normals = new List<vec3>(1024);

			var vertices = new List<vertex>(1024);
			var indices = new List<ushort>(1024);
			var cache = new Dictionary<string, ushort>();

			using (var stream = ManifestResourceManager.LoadResource($"Resources/{model}.obj"))
			{
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						var line = await reader.ReadLineAsync();

						if (string.IsNullOrWhiteSpace(line)) continue;

						if (line[0] == '#') continue;

						if (line.StartsWith("mtllib")) continue;

						if (line[0] == 'o') continue;

						if (line.StartsWith("usemtl")) continue;

						if (line[0] == 's') continue;

						switch (line[0])
						{
							case 'v':
								{
									var pieces = line.Split(' ');

									var vector = new vec3
									(
										Convert.ToSingle(pieces[1], CultureInfo.InvariantCulture),
										Convert.ToSingle(pieces[2], CultureInfo.InvariantCulture),
										pieces.Length > 3 ?
											Convert.ToSingle(pieces[3], CultureInfo.InvariantCulture)
											:
											0.0f
									);

									switch (line[1])
									{
										case ' ': positions.Add(vector); break;
										case 'n': normals.Add(vector); break;
										case 't': /* DoSomething(vector); */ break;
										default: break;
									}
								}
								break;
							case 'f':
								{
									var pieces = line.Split(' ');

									void UseVertex(string vertex)
									{
										var data = vertex.Split('/');

										var pos = Convert.ToInt32(data[0]) - 1;
										var tex = Convert.ToInt32(data[1]) - 1;
										var nor = Convert.ToInt32(data[2]) - 1;

										var vert = new vertex(positions[pos], color, normals[nor]);
										var desc = vert.ToString();

										if (cache.TryGetValue(desc, out ushort index))
										{
											indices.Add(index);
										}
										else
										{
											index = (ushort)vertices.Count;
											vertices.Add(vert);
											indices.Add(index);
											cache.Add(desc, index);
										}
									}

									UseVertex(pieces[1]);
									UseVertex(pieces[2]);
									UseVertex(pieces[3]);
								}
								break;
							default: break;
						}
					}
				}
			}

			return new GeometricShape(new ShapeData(vertices.ToArray(), indices.ToArray()));
		}
	}
}
