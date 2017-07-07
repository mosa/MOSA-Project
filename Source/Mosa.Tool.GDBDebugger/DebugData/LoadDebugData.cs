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

							var section = new SectionInfo()
							{
								Address = parts[0].ParseHex(),
								Offset = parts[1].ToUInt32(),
								Size = parts[2].ToInt32(),
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

							var symbol = new SymbolInfo()
							{
								Address = parts[0].ParseHex(),
								Offset = parts[1].ToUInt32(),
								Length = parts[2].ToInt32(),
								Kind = parts[3],
								Name = parts[4],
							};

							debugSource.Add(symbol);
							continue;
						}

					case "[Types]":
						{
							if (parts[0].StartsWith("TypeDef"))
								continue;

							var type = new TypeInfo()
							{
								DefAddress = parts[0].ParseHex(),
								Size = parts[1].ToUInt32(),
								FullName = parts[2],
								BaseType = parts[3],
								DeclaringType = parts[4],
								ElementType = parts[5],
							};

							debugSource.Add(type);
							continue;
						}

					case "[Methods]":
						{
							if (parts[0].StartsWith("Address"))
								continue;

							var method = new MethodInfo()
							{
								Address = parts[0].ParseHex(),
								Size = parts[1].ToUInt32(),
								DefAddress = parts[2].ParseHex(),
								FullName = parts[3],
								Type = parts[4],
								ReturnType = parts[5],
								StackSize = parts[6].ToUInt32(),
								ParameterStackSize = parts[7].ToUInt32(),
								Attributes = parts[8].ToUInt32(),
							};

							debugSource.Add(method);
							continue;
						}

					case "[Parameters]":
						{
							if (parts[0].StartsWith("Index"))
								continue;

							var parameter = new ParameterInfo()
							{
								Index = parts[0].ToUInt32(),
								Offset = parts[1].ToUInt32(),
								FullName = parts[2],
								Name = parts[3],
								Type = parts[4],
								Method = parts[5],
								Attributes = parts[6].ToUInt32(),
							};

							debugSource.Add(parameter);
							continue;
						}

					default: continue;
				}
			}
		}
	}
}
