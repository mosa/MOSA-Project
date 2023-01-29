// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildIRInstructions : BuildBaseTemplate
{
	public BuildIRInstructions(string jsonFile, string destinationPath, string destinationFile)
		: base(jsonFile, destinationPath, destinationFile)
	{
	}

	protected override void Body()
	{
		Lines.AppendLine("using Mosa.Compiler.Framework;");
		Lines.AppendLine("using System.Collections.Generic;");
		Lines.AppendLine("using Mosa.Compiler.Framework.IR;");
		Lines.AppendLine();
		Lines.AppendLine("namespace Mosa.Compiler.Framework.IR;");
		Lines.AppendLine();
		Lines.AppendLine("/// <summary>");
		Lines.AppendLine("/// IR Instruction Map");
		Lines.AppendLine("/// </summary>");
		Lines.AppendLine("public static class IRInstructions");
		Lines.AppendLine("{");
		Lines.AppendLine("\tpublic static readonly List<BaseInstruction> List = new List<BaseInstruction> {");

		foreach (var entry in Entries.Instructions)
		{
			Lines.AppendLine("\t\tIRInstruction." + entry.Name + ",");
		}

		Lines.AppendLine("\t};");
		Lines.AppendLine("}");
	}
}
