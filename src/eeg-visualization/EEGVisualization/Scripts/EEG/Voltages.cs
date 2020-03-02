using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace EEGVisualization.Scripts.EEG
{
	public static class Voltages
	{
		private const float Speed = 0.1f;

		public static int Frequency { get; private set; } = 1;
		public static int Time { get; private set; } = 190;
		public static float TimePointer { get; private set; } = 0;
		public static uint[] ElectrodeIndices { get; private set; }
		public static List<float[]> Content { get; private set; } = new List<float[]>(1024);
		public static float[] Values
		{
			get
			{
				var vals = Content[Frequency * Time + (int)TimePointer];
				TimePointer += Speed;
				if (TimePointer >= Frequency) TimePointer = 0;
				if (Frequency * Time + (int)TimePointer >= Content.Count) TimePointer = 0;
				return vals;
			}
		}

		public static async Task Init()
		{
			using (var stream = Controller.LoadFromResources("voltages.txt"))
			{
				using (var reader = new StreamReader(stream))
				{
					// Frequency parsing
					var line = await reader.ReadLineAsync();

					if (string.IsNullOrWhiteSpace(line)) throw new FormatException("Frequency not specified.");

					var freq_parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					if (freq_parts.Length != 2) throw new FormatException("Invalid frequency specification.");

					freq_parts[1] = freq_parts[1].Trim();

					if (!freq_parts[1].EndsWith("Hz")) throw new FormatException("Invalid frequency specification.");

					Frequency = (int)Convert.ToSingle(freq_parts[0].Trim(), CultureInfo.InvariantCulture);

					switch (freq_parts[1][0])
					{
						case 'k': Frequency *= 1000; break;
						case 'M': Frequency *= 1000000; break;
						case 'G': Frequency *= 1000000000; break;
					}

					// Electrode name parsing
					line = await reader.ReadLineAsync();
					if (string.IsNullOrWhiteSpace(line)) throw new FormatException("Electrodes not specified.");

					var electrodes = line.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
					ElectrodeIndices = electrodes.Select(name => Electrodes.GetElectrodeIndex(name)).ToArray();

					while (!reader.EndOfStream)
					{
						line = await reader.ReadLineAsync();

						var volts = line
							.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries)
							.Select(v => Convert.ToSingle(v.Trim(), CultureInfo.InvariantCulture))
							.ToArray();

						Content.Add(volts);
					}
				}
			}
		}

		public static void Forward()
		{
			if (Time < (int)Math.Ceiling((float)Content.Count / (float)Frequency) - 1)
			{
				++Time;
				TimePointer = 0;
			}
		}
		public static void Back()
		{
			if (Time > 0)
			{
				--Time;
				TimePointer = 0;
			}
		}
	}
}
