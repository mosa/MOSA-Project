// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildARM32 : BuildBaseTemplate
{
	public BuildARM32(string jsonFile, string destinationPath, string destinationFile)
		: base(jsonFile, destinationPath, destinationFile)
	{
	}

	protected override void Body()
	{
		Lines.AppendLine("using Mosa.Compiler.Framework;");
		Lines.AppendLine("using Mosa.Platform.ARM32.Instructions;");
		Lines.AppendLine();
		Lines.AppendLine("namespace Mosa.Platform.ARM32;");
		Lines.AppendLine();
		Lines.AppendLine("/// <summary>");
		Lines.AppendLine("/// ARM32 Instructions");
		Lines.AppendLine("/// </summary>");
		Lines.AppendLine("public static class ARM32");
		Lines.AppendLine("{");

		foreach (var entry in Entries.Instructions)
		{
			Lines.AppendLine("\tpublic static readonly BaseInstruction " + entry.Name + " = new " + entry.Name + "();");
		}

		Lines.AppendLine("}");
	}
}
