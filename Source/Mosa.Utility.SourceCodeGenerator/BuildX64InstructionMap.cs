// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildX64InstructionMap : BuildBaseTemplate
	{
		public BuildX64InstructionMap(string jsonFile, string destinationPath, string destinationFile)
			: base(jsonFile, destinationPath, destinationFile)
		{
		}

		protected override void Body()
		{
			Lines.AppendLine("using System.Collections.Generic;");
			Lines.AppendLine();
			Lines.AppendLine("namespace Mosa.Platform.x64");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine("\t/// X64 Instruction Map");
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\tpublic static class X64InstructionMap");
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tpublic static readonly Dictionary<string, X64Instruction> Map = new Dictionary<string, X64Instruction>() {");

			foreach (var entry in Entries.Instructions)
			{
				Lines.AppendLine("\t\t\t{ \"" + entry.Name + "\", X64." + entry.Name + " },");
			}

			Lines.AppendLine("\t\t};");
			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
