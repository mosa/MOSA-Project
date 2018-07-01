// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace Mosa.Workspace.Generator.Debug
{
	public class BuildIRInstructionMap : BuildBaseTemplate
	{
		public BuildIRInstructionMap(string jsonFile, string destinationPath, string destinationFile)
			: base(jsonFile, destinationPath, destinationFile)
		{
		}

		protected override void Body()
		{
			Lines.AppendLine("using System.Collections.Generic;");
			Lines.AppendLine();
			Lines.AppendLine("namespace Mosa.Compiler.Framework.IR");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine("\t/// IR Instruction Map");
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\tpublic static class IRInstructionMap");
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tpublic static readonly Dictionary<string, BaseInstruction> Map = new Dictionary<string, BaseInstruction>() {");

			foreach (var entry in Entries.Instructions)
			{
				Lines.AppendLine("\t\t\t{ \"" + entry.Name + "\", IRInstruction." + entry.Name + " },");
			}

			Lines.AppendLine("\t\t};");
			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
