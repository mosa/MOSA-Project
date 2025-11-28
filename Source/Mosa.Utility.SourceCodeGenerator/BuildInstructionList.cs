// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildInstructionList : BuildBaseTemplate
{
	protected virtual string Platform { get; }

	protected virtual string Namespace { get; }

	public BuildInstructionList(string jsonFile, string destinationPath, string destinationFile, string platform, string @namepsace)
		: base(jsonFile, destinationPath, destinationFile)
	{
		Platform = platform;
		Namespace = @namepsace;
	}

	protected override void Body()
	{
		if ($"Mosa.Compiler.{Namespace}" != "Mosa.Compiler.Framework")
		{
			Lines.AppendLine("using Mosa.Compiler.Framework;");
		}

		Lines.AppendLine($"using Mosa.Compiler.{Namespace}.Instructions;");
		Lines.AppendLine();
		Lines.AppendLine($"namespace Mosa.Compiler.{Namespace};");
		Lines.AppendLine();
		Lines.AppendLine("/// <summary>");
		Lines.AppendLine($"/// {Platform} Instructions");
		Lines.AppendLine("/// </summary>");
		Lines.AppendLine($"public static class {Platform}");
		Lines.AppendLine("{");

		foreach (var entry in Entries.Instructions)
		{
			// .NET 9 added a new System.Threading.Lock class, which interferes with instructions named that way
			var className = entry.Name == "Lock" ? "Instructions.Lock" : entry.Name;
			Lines.AppendLine("\tpublic static readonly BaseInstruction " + entry.Name + " = new " + className + "();");
		}

		Lines.AppendLine("}");
	}
}
