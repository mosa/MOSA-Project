// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildIRInstruction : BuildBaseTemplate
{
	public BuildIRInstruction(string jsonFile, string destinationPath, string destinationFile)
		: base(jsonFile, destinationPath, destinationFile)
	{
	}

	protected override void Body()
	{
		Lines.AppendLine("using Mosa.Compiler.Framework;");
		Lines.AppendLine("using Mosa.Compiler.Framework.IR;");
		Lines.AppendLine();
		Lines.AppendLine("namespace Mosa.Compiler.Framework;");
		Lines.AppendLine();
		Lines.AppendLine("/// <summary>");
		Lines.AppendLine("/// IR Instructions");
		Lines.AppendLine("/// </summary>");
		Lines.AppendLine("public static class IRInstruction");
		Lines.AppendLine("{");

		var instructions = Entries.Instructions;

		foreach (var entry in instructions)
		{
			Lines.AppendLine("\tpublic static readonly BaseInstruction " + entry.Name + " = new " + entry.Name + "();");
		}

		Lines.AppendLine("}");
	}
}
