// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildTransformationListFile : BuildBaseTemplate
{
	protected readonly List<string> Filters;
	protected readonly string Namespace;
	protected readonly string Classname;

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
		Lines.AppendLine("using Mosa.Compiler.Framework;");
		Lines.AppendLine();

		Lines.AppendLine($"namespace {Namespace};");
		Lines.AppendLine();
		Lines.AppendLine("/// <summary>");
		Lines.AppendLine("/// Transformations");
		Lines.AppendLine("/// </summary>");
		Lines.AppendLine($"public static class {Classname}");
		Lines.AppendLine("{");
		Lines.AppendLine("\tpublic static readonly List<BaseTransform> List = new List<BaseTransform> {");

		foreach (var name in BuildTransformations.Transformations)
		{
			var include = false;

			foreach (var filter in Filters)
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

				Lines.AppendLine($"\t\tnew {newname}(),");
			}
		}

		Lines.AppendLine("\t};");
		Lines.AppendLine("}");
	}
}
