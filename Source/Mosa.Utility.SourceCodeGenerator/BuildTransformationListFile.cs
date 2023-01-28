// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildTransformationListFile : BuildBaseTemplate
{
	protected List<string> Filters;
	protected string Namespace;
	protected string Classname;

	public BuildTransformationListFile(string destinationPath, string destinationFile, string @namespace, string classname, List<string> filters)
		: base(null, destinationPath, destinationFile)
	{
		Filters = filters;
		Namespace = @namespace;
		Classname = classname;
	}

	protected override void Body()
	{
		Lines.AppendLine("using System.Collections.Generic;");
		Lines.AppendLine("using Mosa.Compiler.Framework.Transforms;");
		Lines.AppendLine();

		Lines.AppendLine($"namespace {Namespace}");
		Lines.AppendLine("{");
		Lines.AppendLine("\t/// <summary>");
		Lines.AppendLine("\t/// Transformations");
		Lines.AppendLine("\t/// </summary>");
		Lines.AppendLine($"\tpublic static class {Classname}");
		Lines.AppendLine("\t{");
		Lines.AppendLine("\t\tpublic static readonly List<BaseTransform> List = new List<BaseTransform> {");

		foreach (var name in BuildTransformations.Transformations)
		{
			bool include = false;

			foreach (string filter in Filters)
			{
				if (name.StartsWith(filter))
				{
					include = true;
					break;
				}

				//var regex = new Regex(filter);

				//var match = regex.Match(name);

				//if (match.Success)
				//{
				//	include = true;
				//	break;
				//}
			}

			if (include)
			{
				var pos = name.IndexOf('.');

				//var newname = name.Replace(Namespace, string.Empty);
				//var newname = name;
				var newname = name.Substring(pos + 1);

				Lines.AppendLine($"\t\t\t new {newname}(),");
			}
		}

		Lines.AppendLine("\t\t};");
		Lines.AppendLine("\t}");
		Lines.AppendLine("}");
	}
}
