// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildRuleFiles : BuildBaseTemplate
	{
		public BuildRuleFiles(string jsonFile, string destinationPath)
			: base(jsonFile, destinationPath)
		{
		}

		protected override void Iterator()
		{
			foreach (var entry in Entries.Rules)
			{
				Lines.Clear();

				var name = entry.Name.Replace(" ", string.Empty);

				DestinationFile = name + ".cs";
				AddSourceHeader();
				Body(entry);
				Save();
			}
		}

		protected override void Body(dynamic node = null)
		{
			int id = Identifiers.GetRuleID();
			var name = node.Name.Replace(" ", string.Empty);

			Lines.AppendLine("using Mosa.Compiler.Framework;");
			Lines.AppendLine("using Mosa.Compiler.Framework.Expression;");

			Lines.AppendLine();
			Lines.AppendLine("namespace Mosa.Compiler.Framework.Rules");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.Append("\t/// " + node.Name);

			if (!string.IsNullOrWhiteSpace(node.Description))
			{
				Lines.Append(" - " + node.Description);
			}

			Lines.AppendLine();
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine("\t/// <seealso cref=\"Mosa.Compiler.Framework.Expression.BaseRule\" />");
			Lines.AppendLine("\tpublic sealed class " + name + " : BaseRule");
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tpublic override int ID { get { return " + id.ToString() + "; } }");
			Lines.AppendLine();
			Lines.AppendLine("\t\tinternal " + node.Name + "()");
			Lines.AppendLine("\t\t{");
			Lines.AppendLine("\t\t}");

			if (node.Name != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override string Name { get { return \"" + node.Name + "\"; } }");
			}

			if (node.Type != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override string Type { get { return \"" + node.Type + "\"; } }");
			}

			if (node.Match != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override string Match { get { return \"" + node.Match + "\"; } }");
			}

			if (node.Transform != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override string Transform { get { return \"" + node.Transform + "\"; } }");
			}

			if (node.Criteria != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override string Criteria { get { return \"" + node.Criteria + "\"; } }");
			}

			if (node.DefaultInstructionFamily != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override string DefaultInstructionFamily { get { return \"" + node.DefaultInstructionFamily + "\"; } }");
			}

			if (node.DefaultArchitectureFamily != null)
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override string DefaultArchitectureFamily { get { return \"" + node.DefaultArchitectureFamily + "\"; } }");
			}

			if (node.Optimization != null && node.Optimization.Contains("true"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsOptimization { get { return true; } }");
			}

			if (node.Transformation != null && node.Transformation.Contains("true"))
			{
				Lines.AppendLine();
				Lines.AppendLine("\t\tpublic override bool IsTransformation { get { return true; } }");
			}

			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
