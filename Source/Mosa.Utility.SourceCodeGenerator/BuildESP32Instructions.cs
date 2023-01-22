// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildESP32Instructions : BuildBaseTemplate
	{
		public BuildESP32Instructions(string jsonFile, string destinationPath, string destinationFile)
			: base(jsonFile, destinationPath, destinationFile)
		{
		}

		protected override void Body()
		{
			Lines.AppendLine("using Mosa.Compiler.Framework;");
			Lines.AppendLine("using System.Collections.Generic;");
			Lines.AppendLine();
			Lines.AppendLine("namespace Mosa.Compiler.Platform.ESP32");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine("\t/// ESP32 Instruction Map");
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\tpublic static class ESP32Instructions");
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tpublic static readonly List<BaseInstruction> List = new List<BaseInstruction> {");

			foreach (var entry in Entries.Instructions)
			{
				Lines.AppendLine("\t\t\tESP32." + entry.Name + ",");
			}

			Lines.AppendLine("\t\t};");
			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
