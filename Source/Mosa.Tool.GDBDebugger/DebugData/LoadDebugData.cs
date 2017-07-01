// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.IO;

namespace Mosa.Tool.GDBDebugger.DebugData
{
	public static class LoadDebugData
	{
		public static void LoadDebugInfo(string filename, DebugSource debugSource)
		{
			var lines = File.ReadLines(filename);

			string fileSection = string.Empty;

			foreach (var line in lines)
			{
				if (string.IsNullOrWhiteSpace(line))
					continue;

				if (line.StartsWith("["))
				{
					fileSection = line.Trim();
					continue;
				}

				var parts = line.Split('\t');

				switch (fileSection)
				{
					case "[Sections]":
						{
							if (parts[0].StartsWith("Address"))
								continue;

							var section = new Section()
							{
								Address = parts[0].ParseHex(),
								Offset = (uint)parts[1].ParseHex(),
								Size = (int)parts[2].ParseHex(),
								Kind = parts[3],
								Name = parts[4],
							};

							debugSource.Add(section);
							continue;
						}

					case "[Symbols]":
						{
							if (parts[0].StartsWith("Address"))
								continue;

							var symbol = new Symbol()
							{
								Address = parts[0].ParseHex(),
								Offset = (uint)parts[1].ParseHex(),
								Size = (int)parts[2].ParseHex(),
								Kind = parts[3],
								Name = parts[4],
							};

							debugSource.Add(symbol);
							continue;
						}
					default: continue;
				}
			}
		}
	}
}
