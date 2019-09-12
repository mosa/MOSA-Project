// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.SourceCodeGenerator.TransformExpressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildTransformations : BuildBaseTemplate
	{
		public static List<string> Transformations = new List<string>();

		public BuildTransformations(string jsonFile, string destinationPath)
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
			Transformations.Add($"{node.FamilyName}.{node.Type}.{node.Name}{node.SubName}");

			//Lines.AppendLine("using Mosa.Compiler.Framework;");
			Lines.AppendLine("using Mosa.Compiler.Framework.IR;");

			//Lines.AppendLine("using Mosa.Compiler.Framework.Transform;");

			Lines.AppendLine();
			Lines.AppendLine($"namespace Mosa.Compiler.Framework.Transform.Auto.{node.FamilyName}.{node.Type}");
			Lines.AppendLine("{");
			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine($"\t/// {node.Name}{node.SubName}");
			Lines.AppendLine("\t/// </summary>");

			Lines.AppendLine($"\tpublic sealed class {node.Name}{node.SubName} : BaseTransformation");
			Lines.AppendLine("\t{");

			var transform = new Transformation(node.Expression, node.Filter, node.Result);
			var instructionName = transform.InstructionTree.InstructionName.Replace("IR.", "IRInstruction.");

			// constructor
			Lines.AppendLine($"\t\tpublic {node.Name}{node.SubName}() : base({instructionName})");
			Lines.AppendLine("\t\t{");
			Lines.AppendLine("\t\t}");
			Lines.AppendLine("");

			Lines.AppendLine("\t\tpublic override bool Match(Context context, TransformContext transformContext)");
			Lines.AppendLine("\t\t{");

			ProcessExpressionNode(transform.InstructionTree);

			ProcessDuplicatesInExpression(transform);

			ProcessFilters(transform);

			Lines.AppendLine("\t\t\treturn true;");
			Lines.AppendLine("\t\t}");

			Lines.AppendLine("");
			Lines.AppendLine("\t\tpublic override void Transform(Context context, TransformContext transformContext)");
			Lines.AppendLine("\t\t{");

			ProcessResultInstructionTree(transform);

			Lines.AppendLine("\t\t}");
			Lines.AppendLine("\t}");
			Lines.AppendLine("}");

			NodeNbrToNode.Clear();
			OperandLabelToVariable.Clear();
		}

		private void ProcessResultInstructionTree(Transformation transform)
		{
			// Capture the result type
			Lines.AppendLine("\t\t\tvar result = context.Result;");
			Lines.AppendLine("");

			// Capture all the labeled operands into variables
			int labelCount = 0;
			var labelToLabelNbr = new Dictionary<string, int>();
			foreach (var name in transform.LabelSet.Labels)
			{
				var label = transform.LabelSet.GetExpressionLabel(name);

				if (!label.IsInResult)
					continue;

				labelCount++;

				var labelPosition = label.Positions[0];
				var labelParent = NodeNbrToNode[labelPosition.NodeNbr];
				var labelOperandName = GetOperandName(labelPosition.OperandIndex);
				var labelName = $"context.{labelParent}{labelOperandName}";

				labelToLabelNbr.Add(label.Name, labelCount);

				Lines.AppendLine($"\t\t\tvar t{labelCount} = {labelName};");
			}
			if (labelCount != 0)
				Lines.AppendLine("");

			var postOrder = transform.GetPostorder(transform.ResultInstructionTree);

			// Create virtual register for each child instruction
			int virtualRegisterNbr = 0;
			var nodeNbrToVirtualRegisterNbr = new Dictionary<int, int>();
			foreach (var node in postOrder)
			{
				if (node == transform.ResultInstructionTree)
					continue;

				virtualRegisterNbr++;
				var resultType = DetermineResultType(node);

				nodeNbrToVirtualRegisterNbr.Add(node.NodeNbr, virtualRegisterNbr);

				Lines.AppendLine($"\t\t\tvar v{virtualRegisterNbr} = transformContext.AllocateVirtualRegister(transformContext.{resultType});");
			}
			if (virtualRegisterNbr != 0)
				Lines.AppendLine("");

			// Create all the constants variables
			var operandList = transform.GetAllOperands(transform.ResultInstructionTree);
			int constantNbr = 0;
			var constantTextToConstantNbr = new Dictionary<string, int>();
			var constantToConstantNbr = new Dictionary<Operand, int>();
			foreach (var operand in operandList)
			{
				string name = CreateConstantName(operand);

				if (name == null)
					continue;

				if (constantTextToConstantNbr.TryGetValue(name, out int found))
				{
					constantToConstantNbr.Add(operand, found);
					Lines.AppendLine($"\t\t\tvar c{operand} = transformContext.CreateConstant({name});");
					continue;
				}

				constantNbr++;
				constantTextToConstantNbr.Add(name, constantNbr);
				constantToConstantNbr.Add(operand, constantNbr);

				Lines.AppendLine($"\t\t\tvar c{constantNbr} = transformContext.CreateConstant({name});");
			}
			if (constantNbr != 0)
				Lines.AppendLine("");

			// Evaluate functions
			int methodNbr = 0;
			var methodToExpressionText = new Dictionary<string, int>();
			var methodToMethodNbr = new Dictionary<Method, int>();
			foreach (var node in postOrder)
			{
				foreach (var operand in node.Operands)
				{
					if (!operand.IsMethod)
						continue;

					string name = CreateExpression(operand.Method, labelToLabelNbr, constantToConstantNbr);

					if (methodToExpressionText.TryGetValue(name, out int found))
					{
						methodToMethodNbr.Add(operand.Method, found);
						Lines.AppendLine($"\t\t\tvar e{found} = transformContext.CreateConstant({name});");
						continue;
					}

					methodNbr++;

					methodToMethodNbr.Add(operand.Method, methodNbr);
					methodToExpressionText.Add(name, methodNbr);

					Lines.AppendLine($"\t\t\tvar e{methodNbr} = transformContext.CreateConstant({name});");
				}
			}
			if (methodNbr != 0)
				Lines.AppendLine("");

			// Create Instructions
			bool firstInstruction = true;
			foreach (var node in postOrder)
			{
				var sb = new StringBuilder();
				foreach (var operand in node.Operands)
				{
					if (operand.IsLabel)
					{
						sb.Append($"t{labelToLabelNbr[operand.LabelName]}");
					}
					else if (operand.IsInteger)
					{
						sb.Append($"c{constantToConstantNbr[operand]}");
					}
					else if (operand.IsLong)
					{
						sb.Append($"c{constantToConstantNbr[operand]}");
					}
					else if (operand.IsDouble)
					{
						sb.Append($"c{constantToConstantNbr[operand]}");
					}
					else if (operand.IsFloat)
					{
						sb.Append($"c{constantToConstantNbr[operand]}");
					}
					else if (operand.IsInstruction)
					{
						sb.Append($"v{nodeNbrToVirtualRegisterNbr[operand.InstructionNode.NodeNbr]}");
					}
					else if (operand.IsMethod)
					{
						var nbr = methodToMethodNbr[operand.Method];
						sb.Append($"e{nbr}");
					}

					sb.Append(", ");
				}

				if (node.Operands.Count != 0)
					sb.Length -= 2;

				var operands = sb.ToString();

				var operation = firstInstruction ? "Set" : "Append";

				if (!string.IsNullOrWhiteSpace(node.InstructionName))
				{
					var instruction = node.InstructionName.Replace("IR.", "IRInstruction."); ;
					var result = node == transform.ResultInstructionTree ? "result" : $"v{nodeNbrToVirtualRegisterNbr[node.NodeNbr]}";

					Lines.AppendLine($"\t\t\tcontext.{operation}Instruction({instruction}, {result}, {operands});");
				}
				else
				{
					Lines.AppendLine($"\t\t\tcontext.{operation}Instruction(GetMove(result), result, {operands});");
				}

				firstInstruction = false;
			}
		}

		private string CreateExpression(Method method, Dictionary<string, int> labelToLabelNbr, Dictionary<Operand, int> constantToConstantNbr)
		{
			var sb = new StringBuilder();

			sb.Append(method.MethodName);
			sb.Append("(");

			foreach (var operand in method.Parameters)
			{
				if (operand.IsLabel)
				{
					var name = labelToLabelNbr[operand.LabelName];
					sb.Append($"t{name}");
				}
				else if (operand.IsInteger)
				{
					sb.Append(CreateConstantName(operand));
				}
				else if (operand.IsLong)
				{
					sb.Append(CreateConstantName(operand));
				}
				else if (operand.IsDouble)
				{
					sb.Append(CreateConstantName(operand));
				}
				else if (operand.IsFloat)
				{
					sb.Append(CreateConstantName(operand));
				}
				else if (operand.IsMethod)
				{
					sb.Append(CreateExpression(operand.Method, labelToLabelNbr, constantToConstantNbr));
				}

				sb.Append(", ");
			}

			if (method.Parameters.Count != 0)
				sb.Length -= 2;

			sb.Append(")");

			return sb.ToString();
		}

		private static string CreateConstantName(Operand operand)
		{
			if (operand.IsInteger)
			{
				return $"{operand.Integer}u";
			}
			else if (operand.IsLong)
			{
				return $"{operand.Long}L";
			}
			else if (operand.IsDouble)
			{
				return $"{operand.Double}d";
			}
			else if (operand.IsFloat)
			{
				return $"{operand.Float}f";
			}

			return null;
		}

		private string DetermineResultType(InstructionNode node)
		{
			if (!string.IsNullOrWhiteSpace(node.ResultType))
				return node.ResultType;

			if (node.InstructionName.EndsWith("32"))
				return "I4";

			if (node.InstructionName.EndsWith("64"))
				return "I8";

			if (node.InstructionName.EndsWith("R4"))
				return "R4";

			if (node.InstructionName.EndsWith("R8"))
				return "R8";

			if (node.InstructionName.EndsWith("Object"))
				return "O";

			// TODO

			return "I8";    // default
		}

		private void ProcessDuplicatesInExpression(Transformation transform)
		{
			foreach (var name in transform.LabelSet.Labels)
			{
				var label = transform.LabelSet.GetExpressionLabel(name);

				if (label.Positions.Count == 1)
					continue;

				var first = label.Positions[0];
				var firstParent = NodeNbrToNode[first.NodeNbr];
				var firstOperandName = GetOperandName(first.OperandIndex);
				var firstName = $"context.{firstParent}{firstOperandName}";

				for (int i = 1; i < label.Positions.Count; i++)
				{
					var other = label.Positions[i];
					var otherParent = NodeNbrToNode[other.NodeNbr];
					var otherOperandName = GetOperandName(other.OperandIndex);
					var otherName = $"context.{otherParent}{otherOperandName}";

					EmitCondition($"!AreSame({firstName}, {otherName})");
				}
			}
		}

		private void ProcessFilters(Transformation transform)
		{
			var filters = transform.Filters;

			foreach (var filter in filters)
			{
				var sb = new StringBuilder();

				if (!filter.IsNegated)
					sb.Append('!');

				sb.Append(ProcessFilters(filter, transform));

				EmitCondition(sb.ToString());
			}
		}

		private string ProcessFilters(Method filter, Transformation transform)
		{
			var sb = new StringBuilder();

			sb.Append(filter.MethodName);
			sb.Append('(');

			foreach (var parameter in filter.Parameters)
			{
				if (parameter.IsMethod)
				{
					sb.Append(ProcessFilters(parameter.Method, transform));
				}
				else if (parameter.IsLabel)
				{
					var first = transform.LabelSet.GetFirstPosition(parameter.Value);

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

			return sb.ToString();
		}

		private void ProcessExpressionNode(InstructionNode instructionNode)
		{
			if (instructionNode.NodeNbr == 0)
			{
				NodeNbrToNode.Add(instructionNode.NodeNbr, string.Empty);
			}

			foreach (var operand in instructionNode.Operands)
			{
				ProcessConditions(operand, instructionNode);
			}

			foreach (var operand in instructionNode.Operands)
			{
				ProcessNestedConditions(operand, instructionNode);
			}
		}

		private void EmitCondition(string condition)
		{
			Lines.AppendLine($"\t\t\tif ({condition})");
			Lines.AppendLine($"\t\t\t\treturn false;");
			Lines.AppendLine("");
		}

		protected void ProcessConditions(Operand operand, InstructionNode parent)
		{
			if (operand.IsAny)
				return; // nothing;

			var operandName = NodeNbrToNode[parent.NodeNbr] + GetOperandName(operand.Index);

			if (operand.IsInstruction)
			{
				EmitCondition($"!context.{operandName}.IsVirtualRegister");
			}

			if (operand.IsLong || operand.IsDouble || operand.IsFloat || operand.IsInteger)
			{
				EmitCondition($"!context.{operandName}.IsResolvedConstant");
			}

			if (operand.IsLong)
			{
				EmitCondition($"context.{operandName}.ConstantUnsigned64 != {operand.Long}");
			}

			if (operand.IsInteger)
			{
				EmitCondition($"context.{operandName}.ConstantUnsigned32 != {operand.Integer}");
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

		protected void ProcessNestedConditions(Operand operand, InstructionNode node)
		{
			if (!operand.IsInstruction)
				return;

			var parent = $"{NodeNbrToNode[node.NodeNbr]}";
			var operandName = GetOperandName(operand.Index);

			EmitCondition($"context.{parent}{operandName}.Definitions.Count != 1");

			var instructionName = operand.InstructionNode.InstructionName.Replace("IR.", "IRInstruction.");

			EmitCondition($"context.{parent}{operandName}.Definitions[0].Instruction != {instructionName}");

			NodeNbrToNode.Add(operand.InstructionNode.NodeNbr, $"{parent}{operandName}.Definitions[0].");

			ProcessExpressionNode(operand.InstructionNode);
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
