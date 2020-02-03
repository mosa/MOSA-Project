// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.IO;

namespace Mosa.Tool.Debugger.DebugData
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
								Length = parts[2].ToUInt32(),
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
								ID = parts[0].ToInt32(),
								DefAddress = parts[1].ParseHex(),
								Size = parts[2].ToUInt32(),
								FullName = parts[3],
								BaseTypeID = parts[4].ToInt32(),
								DeclaringTypeID = parts[5].ToInt32(),
								ElementTypeID = parts[6].ToInt32(),
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
								ID = parts[0].ToInt32(),
								Address = parts[1].ParseHex(),
								Size = parts[2].ToUInt32(),
								DefAddress = parts[3].ParseHex(),
								FullName = parts[4],
								ReturnTypeID = parts[5].ToInt32(),
								StackSize = parts[6].ToUInt32(),
								ParameterStackSize = parts[7].ToUInt32(),
								Attributes = parts[8].ToUInt32(),
								TypeID = parts[9].ToInt32(),
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
								MethodID = parts[0].ToInt32(),
								Index = parts[1].ToUInt32(),
								Offset = parts[2].ToUInt32(),
								Name = parts[3],
								FullName = parts[4],
								ParameterTypeID = parts[5].ToInt32(),
								Attributes = parts[6].ToUInt32(),
								Size = parts[7].ToUInt32(),
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
								TypeID = parts[0].ToInt32(),
								Index = parts[1].ToUInt32(),
								FullName = parts[2],
								Name = parts[3],
								FieldTypeID = parts[4].ToInt32(),
								Address = parts[5].ParseHex(),
								Attributes = parts[6].ToUInt32(),
								Offset = parts[7].ToUInt32(),
								DataLength = parts[8].ToUInt32(),
								DataAddress = parts[9].ParseHex(),
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
								MethodID = parts[0].ToInt32(),
								Label = parts[1].ToInt32(),
								StartOffset = parts[2].ToInt32(),
								Length = parts[3].ToInt32(),
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
								SourceFileID = parts[0].ToInt32(),
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
								MethodID = parts[0].ToInt32(),
								Offset = parts[1].ToInt32(),
								StartLine = parts[2].ToInt32(),
								StartColumn = parts[3].ToInt32(),
								EndLine = parts[4].ToInt32(),
								EndColumn = parts[5].ToInt32(),
								SourceFileID = parts[6].ToInt32()
							};

							debugSource.Add(source);
							continue;
						}

					default: continue;
				}
			}
		}
	}
}
