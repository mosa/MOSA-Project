// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.SourceCodeGenerator.Expression;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildTransformationFiles : BuildBaseTemplate
	{
		public BuildTransformationFiles(string jsonFile, string destinationPath)
			: base(jsonFile, destinationPath)
		{
		}

		protected Dictionary<int, string> NodeNbrToNode = new Dictionary<int, string>();

		protected Dictionary<string, string> OperandLabelToVariable = new Dictionary<string, string>();

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

			ProcessExpressionNode(transform.ExpressionNode, string.Empty);

			ProcessDuplicatesInExpression(transform);

			ProcessFilters(transform);

			Lines.AppendLine("\t\t\treturn true;");
			Lines.AppendLine("\t\t}");

			Lines.AppendLine("");
			Lines.AppendLine("\t\tpublic override void Transform(Context context, TransformContext transformContext)");
			Lines.AppendLine("\t\t{");
			Lines.AppendLine("\t\t}");

			Lines.AppendLine("\t}");
			Lines.AppendLine("}");

			NodeNbrToNode.Clear();
			OperandLabelToVariable.Clear();
		}

		private void ProcessDuplicatesInExpression(Transformation transform)
		{
			foreach (var label in transform.ExpressionLabels.Labels)
			{
				var expressionLabel = transform.ExpressionLabels.GetExpressionLabel(label);

				if (expressionLabel.Positions.Count == 1)
					continue;

				var first = expressionLabel.Positions[0];
				var firstParent = NodeNbrToNode[first.NodeNbr];
				var firstOperandName = GetOperandName(first.OperandIndex);
				var firstName = $"context.{firstParent}{firstOperandName}";

				for (int i = 1; i < expressionLabel.Positions.Count; i++)
				{
					var other = expressionLabel.Positions[i];
					var otherParent = NodeNbrToNode[other.NodeNbr];
					var otherOperandName = GetOperandName(other.OperandIndex);
					var otherName = $"context.{otherParent}{otherOperandName}";

					EmitCondition($"!AreSame({firstName}, {otherName})");
				}
			}
		}

		private void ProcessFilters(Transformation transform)
		{
			var filters = transform.ExpressionMethodFilters;

			foreach (var filter in filters)
			{
				ProcessFilters(filter, transform);
			}
		}

		private void ProcessFilters(ExpressionMethodFilter filter, Transformation transform)
		{
			var sb = new StringBuilder();

			if (!filter.IsNegated)
				sb.Append('!');

			sb.Append(filter.MethodName);
			sb.Append('(');

			foreach (var parameter in filter.Parameters)
			{
				if (parameter.IsMethod)
				{
					ProcessFilters(parameter.Method, transform);
				}
				else if (parameter.IsLabel)
				{
					var first = transform.ExpressionLabels.GetFirstPosition(parameter.Value);

					var parent = NodeNbrToNode[first.NodeNbr];

					var operandName = GetOperandName(first.OperandIndex);

					sb.Append($"context.{parent}{operandName}");
				}
				else if (parameter.IsLong)
				{
					sb.Append(parameter.Long.ToString());
				}
				else if (parameter.IsInteger)
				{
					sb.Append(parameter.Integer.ToString());
				}
				else if (parameter.IsDouble)
				{
					sb.Append(parameter.Double.ToString());
				}
				else if (parameter.IsFloat)
				{
					sb.Append(parameter.Float.ToString());
					sb.Append('f');
				}

				sb.Append(", ");
			}

			if (filter.Parameters.Count != 0)
				sb.Length -= 2;

			sb.Append(')');

			EmitCondition(sb.ToString());
		}

		private void ProcessExpressionNode(ExpressionNode expressionNode, string parent)
		{
			NodeNbrToNode.Add(expressionNode.NodeNbr, parent);

			foreach (var operand in expressionNode.Operands)
			{
				ProcessConditions(operand, parent, expressionNode.NodeNbr);
			}

			foreach (var operand in expressionNode.Operands)
			{
				ProcessNestedConditions(operand, parent, expressionNode.NodeNbr);
			}
		}

		private void EmitCondition(string condition)
		{
			Lines.AppendLine($"\t\t\tif ({condition})");
			Lines.AppendLine($"\t\t\t\treturn false;");
			Lines.AppendLine("");
		}

		protected void ProcessConditions(ExpressionOperand operand, string parent, int nodeNbr)
		{
			if (operand.IsAny)
				return; // nothing;

			var operandName = parent + GetOperandName(operand.Index);

			if (operand.IsExpressionNode)
			{
				EmitCondition($"!context.{operandName}.IsVirtualRegister");
			}

			if (operand.IsLong || operand.IsDouble || operand.IsFloat || operand.IsInteger)
			{
				EmitCondition($"!context.{operandName}.IsResolvedConstant");
			}

			if (operand.IsLong)
			{
				EmitCondition($"context.{operandName}.ConstantUnsignedLongInteger != {operand.Long}");
			}

			if (operand.IsInteger)
			{
				EmitCondition($"context.{operandName}.ConstantUnsignedInteger != {operand.Integer}");
			}

			if (operand.IsDouble)
			{
				EmitCondition($"context.{operandName}.ConstantDouble != {operand.Double}");
			}

			if (operand.IsFloat)
			{
				EmitCondition($"context.{operandName}.ConstantFloat != {operand.Float}f");
			}
		}

		protected void ProcessNestedConditions(ExpressionOperand operand, string parent, int nodenbr)
		{
			if (!operand.IsExpressionNode)
				return;

			var operandName = parent + GetOperandName(operand.Index);

			EmitCondition($"context.{operandName}.Definitions.Count != 1");

			var instructionName = operand.Node.InstructionName.Replace("IR.", "IRInstruction.");

			EmitCondition($"context.{operandName}.Definitions[0].Instruction != {instructionName}");

			ProcessExpressionNode(operand.Node, $"{parent}{operandName}.Definitions[0].");
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
