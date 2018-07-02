// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildARMv6InstructionMap : BuildBaseTemplate
	{
		public BuildARMv6InstructionMap(string jsonFile, string destinationPath, string destinationFile)
			: base(jsonFile, destinationPath, destinationFile)
		{
		}

		protected override void Body()
		{
			Lines.AppendLine("using System.Collections.Generic;");
			Lines.AppendLine();
			Lines.AppendLine("namespace Mosa.Platform.ARMv6");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine("\t/// ARMv6 Instruction Map");
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\tpublic static class ARMv6InstructionMap");
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tpublic static readonly Dictionary<string, ARMv6Instruction> Map = new Dictionary<string, ARMv6Instruction>() {");

			foreach (var entry in Entries.Instructions)
			{
				Lines.AppendLine("\t\t\t{ \"" + entry.Name + "\", ARMv6." + entry.Name + " },");
			}

			Lines.AppendLine("\t\t};");
			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
