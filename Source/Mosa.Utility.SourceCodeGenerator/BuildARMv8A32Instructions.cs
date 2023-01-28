// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildARMv8A32Instructions : BuildBaseTemplate
{
	public BuildARMv8A32Instructions(string jsonFile, string destinationPath, string destinationFile)
		: base(jsonFile, destinationPath, destinationFile)
	{
	}

	protected override void Body()
	{
		Lines.AppendLine("using Mosa.Compiler.Framework;");
		Lines.AppendLine("using System.Collections.Generic;");
		Lines.AppendLine();
		Lines.AppendLine("namespace Mosa.Platform.ARMv8A32");
		Lines.AppendLine("{");
		Lines.AppendLine("\t/// <summary>");
		Lines.AppendLine("\t/// ARMv8A32 Instruction Map");
		Lines.AppendLine("\t/// </summary>");
		Lines.AppendLine("\tpublic static class ARMv8A32Instructions");
		Lines.AppendLine("\t{");
		Lines.AppendLine("\t\tpublic static readonly List<BaseInstruction> List = new List<BaseInstruction> {");

		foreach (var entry in Entries.Instructions)
		{
			Lines.AppendLine("\t\t\tARMv8A32." + entry.Name + ",");
		}

		Lines.AppendLine("\t\t};");
		Lines.AppendLine("\t}");
		Lines.AppendLine("}");
	}
}
