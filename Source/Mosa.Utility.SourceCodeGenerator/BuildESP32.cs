// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildESP32 : BuildBaseTemplate
	{
		public BuildESP32(string jsonFile, string destinationPath, string destinationFile)
			: base(jsonFile, destinationPath, destinationFile)
		{
		}

		protected override void Body()
		{
			Lines.AppendLine("using Mosa.Platform.ESP32.Instructions;");
			Lines.AppendLine();
			Lines.AppendLine("namespace Mosa.Platform.ESP32");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine("\t/// ESP32 Instructions");
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\tpublic static class ESP32");
			Lines.AppendLine("\t{");

			foreach (var entry in Entries.Instructions)
			{
				Lines.AppendLine("\t\tpublic static readonly " + entry.Name + " " + entry.Name + " = new " + entry.Name + "();");
			}

			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
