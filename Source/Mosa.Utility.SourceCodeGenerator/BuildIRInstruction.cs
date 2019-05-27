// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildIRInstruction : BuildBaseTemplate
	{
		public BuildIRInstruction(string jsonFile, string destinationPath, string destinationFile)
			: base(jsonFile, destinationPath, destinationFile)
		{
		}

		protected override void Body()
		{
			Lines.AppendLine("namespace Mosa.Compiler.Framework.IR");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine("\t/// IR Instructions");
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\tpublic static class IRInstruction");
			Lines.AppendLine("\t{");

			var instructions = Entries.Instructions;

			foreach (var entry in instructions)
			{
				Lines.AppendLine("\t\tpublic static readonly " + entry.Name + " " + entry.Name + " = new " + entry.Name + "();");
			}

			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
