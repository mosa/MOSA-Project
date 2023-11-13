// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildInstructionsList : BuildBaseTemplate
{
	private readonly string Platform;   // X86
	private readonly string Namespace;  // x86

	public BuildInstructionsList(string jsonFile, string destinationPath, string destinationFile, string platform, string @namespace)
		: base(jsonFile, destinationPath, destinationFile)
	{
		Namespace = @namespace;
		Platform = platform;
	}

	protected override void Body()
	{
		Lines.AppendLine("using Mosa.Compiler.Framework;");
		Lines.AppendLine();
		Lines.AppendLine($"namespace Mosa.Compiler.{Namespace};");
		Lines.AppendLine();
		Lines.AppendLine("/// <summary>");
		Lines.AppendLine($"/// {Namespace} Instruction Map");
		Lines.AppendLine("/// </summary>");
		Lines.AppendLine($"public static class {Platform}Instructions");
		Lines.AppendLine("{");
		Lines.AppendLine("\tpublic static readonly List<BaseInstruction> List = new() {");

		foreach (var entry in Entries.Instructions)
		{
			Lines.AppendLine($"\t\t{Platform}.{entry.Name},");
		}

		Lines.AppendLine("\t};");
		Lines.AppendLine("}");
	}
}
