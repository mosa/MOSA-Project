// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Globalization;

namespace Mosa.Tool.Debugger.DebugData;

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
							Address = ulong.Parse(parts[0], NumberStyles.HexNumber),
							Offset = Convert.ToUInt32(parts[1]),
							Size = Convert.ToInt32(parts[2]),
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
							Address = ulong.Parse(parts[0], NumberStyles.HexNumber),
							Offset = Convert.ToUInt32(parts[1]),
							Length = Convert.ToUInt32(parts[2]),
							Kind = parts[3],
							Name = parts[4],
						};

						debugSource.Add(symbol);
						continue;
					}

				case "[Types]":
					{
						if (parts[0].StartsWith("TypeID"))
							continue;

						var type = new TypeInfo()
						{
							ID = Convert.ToInt32(parts[0]),
							DefAddress = ulong.Parse(parts[1], NumberStyles.HexNumber),
							Size = Convert.ToUInt32(parts[2]),
							FullName = parts[3],
							BaseTypeID = Convert.ToInt32(parts[4]),
							DeclaringTypeID = Convert.ToInt32(parts[5]),
							ElementTypeID = Convert.ToInt32(parts[6]),
						};

						debugSource.Add(type);
						continue;
					}

				case "[Methods]":
					{
						if (parts[0].StartsWith("MethodID"))
							continue;

						var method = new MethodInfo()
						{
							ID = Convert.ToInt32(parts[0]),
							Address = ulong.Parse(parts[1], NumberStyles.HexNumber),
							Size = Convert.ToUInt32(parts[2]),
							DefAddress = ulong.Parse(parts[3], NumberStyles.HexNumber),
							FullName = parts[4],
							ReturnTypeID = Convert.ToInt32(parts[5]),
							StackSize = Convert.ToUInt32(parts[6]),
							ParameterStackSize = Convert.ToUInt32(parts[7]),
							Attributes = Convert.ToUInt32(parts[8]),
							TypeID = Convert.ToInt32(parts[9]),
						};

						debugSource.Add(method);
						continue;
					}

				case "[Parameters]":
					{
						if (parts[0].StartsWith("MethodID"))
							continue;

						var parameter = new ParameterInfo()
						{
							MethodID = Convert.ToInt32(parts[0]),
							Index = Convert.ToUInt32(parts[1]),
							Offset = Convert.ToUInt32(parts[2]),
							Name = parts[3],
							FullName = parts[4],
							ParameterTypeID = Convert.ToInt32(parts[5]),
							Attributes = Convert.ToUInt32(parts[6]),
							Size = Convert.ToUInt32(parts[7]),
						};

						debugSource.Add(parameter);
						continue;
					}

				case "[Fields]":
					{
						if (parts[0].StartsWith("TypeID"))
							continue;

						var field = new FieldInfo()
						{
							TypeID = Convert.ToInt32(parts[0]),
							Index = Convert.ToUInt32(parts[1]),
							FullName = parts[2],
							Name = parts[3],
							FieldTypeID = Convert.ToInt32(parts[4]),
							Address = ulong.Parse(parts[5], NumberStyles.HexNumber),
							Attributes = Convert.ToUInt32(parts[6]),
							Offset = Convert.ToUInt32(parts[7]),
							DataLength = Convert.ToUInt32(parts[8]),
							DataAddress = ulong.Parse(parts[9], NumberStyles.HexNumber),
						};

						debugSource.Add(field);
						continue;
					}
				case "[SourceLabels]":
					{
						if (parts[0].StartsWith("MethodID"))
							continue;

						var sourceLabel = new SourceLabelInfo()
						{
							MethodID = Convert.ToInt32(parts[0]),
							Label = Convert.ToInt32(parts[1]),
							StartOffset = Convert.ToInt32(parts[2]),
							Length = Convert.ToInt32(parts[3]),
						};

						debugSource.Add(sourceLabel);
						continue;
					}
				case "[SourceFile]":
					{
						if (parts[0].StartsWith("SourceFileID"))
							continue;

						var file = new SourceFileInfo()
						{
							SourceFileID = Convert.ToInt32(parts[0]),
							Filename = parts[1]
						};

						debugSource.Add(file);
						continue;
					}
				case "[Source]":
					{
						if (parts[0].StartsWith("MethodID"))
							continue;

						var source = new SourceInfo()
						{
							MethodID = Convert.ToInt32(parts[0]),
							Offset = Convert.ToInt32(parts[1]),
							StartLine = Convert.ToInt32(parts[2]),
							StartColumn = Convert.ToInt32(parts[3]),
							EndLine = Convert.ToInt32(parts[4]),
							EndColumn = Convert.ToInt32(parts[5]),
							SourceFileID = Convert.ToInt32(parts[6])
						};

						debugSource.Add(source);
						continue;
					}

				default: continue;
			}
		}
	}
}
