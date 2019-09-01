// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.SourceCodeGenerator.Expression;
using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildTransformationFiles : BuildBaseTemplate
	{
		public BuildTransformationFiles(string jsonFile, string destinationPath)
			: base(jsonFile, destinationPath)
		{
		}

		protected override void Iterator()
		{
			foreach (var entry in Entries.Optimizations)
			{
				Lines.Clear();

				DestinationFile = $"{entry.FamilyName}\\{entry.Type}\\{entry.Name}{entry.SubName}.cs";
				AddSourceHeader();
				Body(entry);
				Save();
			}
		}

		protected override void Body(dynamic node = null)
		{
			Lines.AppendLine("using Mosa.Compiler.Framework;");
			Lines.AppendLine("using Mosa.Compiler.Framework.IR;");
			Lines.AppendLine("using Mosa.Compiler.Framework.Transformation;");

			Lines.AppendLine();
			Lines.AppendLine($"namespace Mosa.Compiler.Framework.Transformation.{node.FamilyName}.{node.Type}");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine($"\t/// {node.Name}{node.SubName}");
			Lines.AppendLine("\t/// </summary>");

			Lines.AppendLine($"\tpublic sealed class {node.Name}{node.SubName} : BaseTransformation");
			Lines.AppendLine("\t{");

			var transform = new Transformation(node.Expression, node.Filter, node.Result);

			var instructionName = transform.ExpressionNode.InstructionName.Replace("IR.", "IRInstruction.");

			// constructor
			Lines.AppendLine($"\t\tpublic {node.Name}{node.SubName}() : base({instructionName})");
			Lines.AppendLine("\t\t{");
			Lines.AppendLine("\t\t}");
			Lines.AppendLine("");

			Lines.AppendLine("\t\tpublic override bool Match(Context context, TransformContext transformContext)");
			Lines.AppendLine("\t\t{");

			for (int index = 0; index < transform.ExpressionNode.Operands.Count; index++)
			{
				var match = CreateConditions(index, transform.ExpressionNode.Operands[index]);

				if (match != null)
				{
					foreach (string condition in match)
					{
						Lines.AppendLine($"\t\t\tif ({condition})");
						Lines.AppendLine($"\t\t\t\treturn false;");
						Lines.AppendLine("");
					}
				}
			}

			Lines.AppendLine("\t\t\treturn true;");
			Lines.AppendLine("\t\t}");

			Lines.AppendLine("");
			Lines.AppendLine("\t\tpublic override void Transform(Context context, TransformContext transformContext)");
			Lines.AppendLine("\t\t{");
			Lines.AppendLine("\t\t}");

			Lines.AppendLine("\t}");
			Lines.AppendLine("}");
		}

		protected List<string> CreateConditions(int index, ExpressionOperand operand)
		{
			if (operand.IsAny)
				return null; // nothing;

			var lines = new List<string>();

			if (operand.IsVirtualRegister)
			{
				lines.Add($"!context.{GetOperandName(index)}.IsVirtualRegister");
			}

			if (operand.IsLong || operand.IsDouble || operand.IsFloat || operand.IsInteger)
			{
				lines.Add($"!context.{GetOperandName(index)}.IsResolvedConstant");
			}

			if (operand.IsLong)
			{
				lines.Add($"context.{GetOperandName(index)}.ConstantUnsignedLongInteger != {operand.Long}");
			}

			if (operand.IsInteger)
			{
				lines.Add($"context.{GetOperandName(index)}.ConstantUnsignedInteger != {operand.Integer}");
			}

			if (operand.IsDouble)
			{
				lines.Add($"context.{GetOperandName(index)}.ConstantDouble != {operand.Double}");
			}

			if (operand.IsFloat)
			{
				lines.Add($"context.{GetOperandName(index)}.ConstantFloat != {operand.Float}f");
			}

			return lines;
		}

		protected string GetOperandName(int index)
		{
			if (index < 3)
				return $"Operand{index + 1}";
			else
				return "GetOperand({index})";
		}
	}
}
