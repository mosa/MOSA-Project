// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mosa.Utility.SourceCodeGenerator
{
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
			//Lines.AppendLine("using Mosa.Compiler.Framework;");
			Lines.AppendLine("using System.Collections.Generic;");
			Lines.AppendLine();

			Lines.AppendLine($"namespace {Namespace}");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine("\t/// Transformations");
			Lines.AppendLine("\t/// </summary>");
			Lines.AppendLine($"\tpublic static class {Classname}");
			Lines.AppendLine("\t{");
			Lines.AppendLine("\t\tpublic static readonly List<BaseTransformation> List = new List<BaseTransformation> {");

			foreach (var name in BuildTransformations.Transformations)
			{
				bool include = false;

				foreach (string filter in Filters)
				{
					var regex = new Regex(filter);

					var match = regex.Match(name);

					if (match.Success)
					{
						include = true;
						break;
					}
				}

				if (include)
				{
					var newname = name.Replace(Namespace, string.Empty);
					var newname2 = name.Replace(".", "_");

					//Lines.AppendLine($"\t\t\tnew {newname}(),");
					Lines.AppendLine($"\t\t\tAutoInstance.{newname2},");
				}
			}

			Lines.AppendLine("\t\t};");
			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}
	}
}
