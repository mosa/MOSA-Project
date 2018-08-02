// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildRules : BuildBaseTemplate
	{
		public BuildRules(string jsonFile, string destinationPath, string destinationFile)
			: base(jsonFile, destinationPath, destinationFile)
		{
		}

		protected override void Body()
		{
			Lines.AppendLine("using Mosa.Compiler.Framework;");
			Lines.AppendLine("using System.Collections.Generic;");
			Lines.AppendLine("using Mosa.Compiler.Framework.Expression;");

			Lines.AppendLine();
			Lines.AppendLine("namespace Mosa.Compiler.Framework");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine("\t/// Rules");
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\tpublic static class Rules");
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tpublic static readonly List<Rule> List = new List<Rule>() {");

			foreach (var entry in Entries.Rules)
			{
				Body(entry);
			}

			Lines.AppendLine("\t\t};");
			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}

		protected override void Body(dynamic node = null)
		{
			int id = Identifiers.GetRuleID();

			Lines.AppendLine("\t\t\tnew Rule() {");

			Lines.AppendLine("\t\t\t\tName = \"" + node.Name + "\",");

			if (node.Type != null)
			{
				Lines.AppendLine("\t\t\t\tType = \"" + node.Type + "\",");
			}

			if (node.Match != null)
			{
				Lines.AppendLine("\t\t\t\tMatch = \"" + node.Match.Replace("\n", " ") + "\",");
			}

			if (node.Transform != null)
			{
				Lines.AppendLine("\t\t\t\tTransform = \"" + node.Transform.Replace("\n", " ") + "\",");
			}

			if (node.Criteria != null)
			{
				Lines.AppendLine("\t\t\t\tCriteria = \"" + node.Criteria.Replace("\n", " ") + "\",");
			}

			if (node.DefaultInstructionFamily != null)
			{
				Lines.AppendLine("\t\t\t\tDefaultInstructionFamily = \"" + node.DefaultInstructionFamily + "\",");
			}

			if (node.DefaultArchitectureFamily != null)
			{
				Lines.AppendLine("\t\t\t\tDefaultArchitectureFamily = \"" + node.DefaultArchitectureFamily + "\",");
			}

			if (node.Optimization != null)
			{
				Lines.AppendLine("\t\t\t\tIsOptimization  = " + (node.Optimization.ToLower().Contains("yes") ? "true" : "false") + ",");
			}

			if (node.Transformation != null)
			{
				Lines.AppendLine("\t\t\t\tIsTransformation  = " + (node.Transformation.ToLower().Contains("yes") ? "true" : "false") + ",");
			}

			Lines.AppendLine("\t\t\t},");
		}
	}
}
