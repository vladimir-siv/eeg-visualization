using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

using GlmNet;

namespace EEGVisualization.Scripts.EEG
{
	using Scripts;

	public static class Electrodes
	{
		private struct Electrode
		{
			public uint Index { get; }
			public float Phi { get; }
			public float Theta { get; }

			public vec3 NormalizedVector
			{
				get
				{
					var sinPhi = glm.sin(Phi);
					var cosPhi = glm.cos(Phi);
					var sinTheta = glm.sin(Theta);
					var cosTheta = glm.cos(Theta);

					return new vec3(sinPhi * cosTheta, sinPhi * sinTheta, cosPhi);
				}
			}

			public Electrode(uint index, float phi, float theta)
			{
				Index = index;
				Phi = phi * (float)Math.PI / 180.0f;
				Theta = theta * (float)Math.PI / 180.0f;
			}
		}

		private static Dictionary<string, uint> NameToIndexCache { get; } = new Dictionary<string, uint>();
		private static List<Electrode> CachedElectrodes { get; } = new List<Electrode>(512);

		public static int Count => CachedElectrodes.Count;
		public static float[] Data
		{
			get
			{
				var data = new float[CachedElectrodes.Count * 3];

				for (int i = 0; i < CachedElectrodes.Count; ++i)
				{
					var vec = CachedElectrodes[i].NormalizedVector;
					data[i * 3 + 0] = vec.x;
					data[i * 3 + 1] = vec.y;
					data[i * 3 + 2] = vec.z;
				}

				return data;
			}
		}

		public static async Task Load()
		{
			using (var stream = Controller.LoadFromResources("electrodes.txt"))
			{
				using (var reader = new StreamReader(stream))
				{
					while (!reader.EndOfStream)
					{
						var line = await reader.ReadLineAsync();
						if (string.IsNullOrWhiteSpace(line)) continue;

						var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

						if (parts.Length != 4) throw new FormatException("Invalid format (4 parts expected).");

						if (parts[0].Trim() != "EEG") throw new FormatException("First parameter not \"EEG\".");

						var name = parts[1].Trim();

						if (NameToIndexCache.ContainsKey(name)) throw new FormatException("Electrode name already present.");

						var index = (uint)CachedElectrodes.Count;
						var phi = Convert.ToSingle(parts[2].Trim(), CultureInfo.InvariantCulture);
						var theta = Convert.ToSingle(parts[3].Trim(), CultureInfo.InvariantCulture);

						NameToIndexCache.Add(name, index);
						CachedElectrodes.Add(new Electrode(index, phi, theta));
					}
				}
			}
		}

		public static uint GetElectrodeIndex(string electrodeName) => NameToIndexCache[electrodeName];
	}
}
