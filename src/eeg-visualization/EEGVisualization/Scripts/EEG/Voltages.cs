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
		private static Stream Stream { get; set; }
		private static StreamReader Reader { get; set; }
		private static LinkedList<float[]> StreamContent { get; set; } = new LinkedList<float[]>();
		private static LinkedListNode<float[]> CurrentNode { get; set; }
		
		public static uint Frequency { get; private set; } = 1u;
		public static uint[] ElectrodeIndices { get; private set; }
		public static float[] Values => CurrentNode.Value;

		public static async Task Init()
		{
			Stream = Controller.LoadFromResources("voltages.txt");
			Reader = new StreamReader(Stream);
			
			// Frequency parsing
			var line = await Reader.ReadLineAsync();

			if (string.IsNullOrWhiteSpace(line)) throw new FormatException("Frequency not specified.");

			var freq_parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			if (freq_parts.Length != 2) throw new FormatException("Invalid frequency specification.");

			freq_parts[1] = freq_parts[1].Trim();

			if (!freq_parts[1].EndsWith("Hz")) throw new FormatException("Invalid frequency specification.");

			Frequency = (uint)Convert.ToSingle(freq_parts[0].Trim(), CultureInfo.InvariantCulture);

			switch (freq_parts[1][0])
			{
				case 'k': Frequency *= 1000; break;
				case 'M': Frequency *= 1000000; break;
				case 'G': Frequency *= 1000000000; break;
			}

			// Electrode name parsing
			line = await Reader.ReadLineAsync();
			if (string.IsNullOrWhiteSpace(line)) throw new FormatException("Electrodes not specified.");

			var electrodes = line.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
			ElectrodeIndices = electrodes.Select(name => Electrodes.GetElectrodeIndex(name)).ToArray();

			await ReadValues();
			CurrentNode = StreamContent.First;
		}

		public static void Back()
		{
			if (CurrentNode == StreamContent.First) return;
			CurrentNode = CurrentNode.Previous;
		}

		public static async Task Forward()
		{
			if (CurrentNode == StreamContent.Last)
			{
				await ReadValues();
				CurrentNode = StreamContent.Last;
			}
			else CurrentNode = CurrentNode.Next;
		}

		private static async Task ReadValues()
		{
			if (Reader.EndOfStream) return;

			var vals = new float[ElectrodeIndices.Length];

			for (int c = 0; c < vals.Length; ++c) vals[c] = 0.0f;

			for (int i = 0; i < Frequency; ++i)
			{
				var line = await Reader.ReadLineAsync();

				var voltages = line.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

				for (int c = 0; c < vals.Length; ++c)
				{
					vals[c] += Convert.ToSingle(voltages[c].Trim(), CultureInfo.InvariantCulture);
				}
			}

			for (int c = 0; c < vals.Length; ++c) vals[c] /= Frequency;

			StreamContent.AddLast(vals);
		}

		public static void Dispose()
		{
			Reader?.Dispose();
			Stream?.Dispose();
		}
	}
}
