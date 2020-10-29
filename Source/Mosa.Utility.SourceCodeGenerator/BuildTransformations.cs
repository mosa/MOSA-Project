// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.SourceCodeGenerator.TransformExpressions;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Utility.SourceCodeGenerator
{
	public class BuildTransformations : BuildBaseTemplate
	{
		public static List<string> Transformations = new List<string>();

		protected List<string> CommutativeInstructions = new List<string>();

		protected Dictionary<int, string> NodeNbrToNode = new Dictionary<int, string>();

		protected Dictionary<string, string> OperandLabelToVariable = new Dictionary<string, string>();

		protected bool First = true;

		public BuildTransformations(string jsonFile, string destinationPath)
			: base(jsonFile, destinationPath)
		{
		}

		protected override void Iterator()
		{
			if (Entries.Commutative != null)
			{
				foreach (string instruction in Entries.Commutative)
				{
					CommutativeInstructions.Add(instruction);
				}
			}

			foreach (var entry in Entries.Optimizations)
			{
				Body(entry);
			}
		}

		protected override void Body(dynamic node = null)
		{
			string name = node.Name;
			string familyName = node.FamilyName;
			string type = node.Type;
			string subName = node.SubName;
			string expression = node.Expression;
			string filter = node.Filter;
			string result = node.Result;
			bool log = (node.Log != null && node.Log == "Yes");
			bool Variations = (node.Variations != null && node.Variations == "Yes");

			GenerateTranformations(name, familyName, type, subName, expression, filter, result, Variations, log);
		}

		private void GenerateTranformations(string name, string familyName, string type, string subName, string expression, string filter, string result, bool Variations, bool log)
		{
			if (expression.Contains("R#"))
			{
				GenerateTransformation(R4(name), R4(familyName), R4(type), R4(subName), new Transformation(R4(expression), R4(filter), R4(result)), Variations, log);
				GenerateTransformation(R8(name), R8(familyName), R8(type), R8(subName), new Transformation(R8(expression), R8(filter), R8(result)), Variations, log);
			}
			else if (expression.Contains("##"))
			{
				GenerateTransformation(To32(name), To32(familyName), To32(type), To32(subName), new Transformation(To32(expression), To32(filter), To32(result)), Variations, log);
				GenerateTransformation(To64(name), To64(familyName), To64(type), To64(subName), new Transformation(To64(expression), To64(filter), To64(result)), Variations, log);
			}
			else
			{
				GenerateTransformation(name, familyName, type, subName, new Transformation(expression, filter, result), Variations, log);
			}
		}

		private static string To32(string s)
		{
			return s?.Replace("##", "32");
		}

		private static string To64(string s)
		{
			return s?.Replace("##", "64");
		}

		private static string R4(string s)
		{
			return s?.Replace("R#", "R4");
		}

		private static string R8(string s)
		{
			return s?.Replace("R#", "R8");
		}

		private void GenerateTransformation(string name, string familyName, string type, string subName, Transformation transform, bool Variations, bool log)
		{
			Lines.Clear();
			First = true;

			DestinationFile = $"{familyName}\\{type}\\{name}{subName}.cs";
			AddSourceHeader();

			Lines.AppendLine("using Mosa.Compiler.Framework.IR;");

			Lines.AppendLine();
			Lines.AppendLine($"namespace Mosa.Compiler.Framework.Transform.Auto.{familyName}.{type}");
			Lines.AppendLine("{");

			GenerateTransformations(name, familyName, type, subName, transform, Variations, log);

			Lines.AppendLine("}");

			Save();
		}

		private void GenerateTransformations(string name, string familyName, string type, string subName, Transformation transform, bool Variations, bool log)
		{
			GenerateTransformation2(name, familyName, type, subName, transform, log);

			if (!Variations)
				return;

			if (CommutativeInstructions == null || CommutativeInstructions.Count == 0)
				return;

			var variations = transform.DeriveVariations(CommutativeInstructions);

			int index = 1;
			foreach (var variation in variations)
			{
				GenerateTransformation2(name, familyName, type, $"{subName}_v{index}", variation, log);
				index++;
			}
		}

		private void GenerateTransformation2(string name, string familyName, string type, string subName, Transformation transform, bool log)
		{
			var instructionName = transform.InstructionTree.InstructionName.Replace("IR.", "IRInstruction.");

			Transformations.Add($"{familyName}.{type}.{name}{subName}");

			if (First)
			{
				First = false;
			}
			else
			{
				Lines.AppendLine();
			}

			Lines.AppendLine("\t/// <summary>");
			Lines.AppendLine($"\t/// {name}{subName}");
			Lines.AppendLine("\t/// </summary>");

			Lines.AppendLine($"\tpublic sealed class {name}{subName} : BaseTransformation");
			Lines.AppendLine("\t{");

			if (log)
				Lines.AppendLine($"\t\tpublic {name}{subName}() : base({instructionName}, true)");
			else
				Lines.AppendLine($"\t\tpublic {name}{subName}() : base({instructionName})");

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
				Lines.AppendLine();

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
						continue;

						//Lines.AppendLine($"\t\t\tvar e{found} = transformContext.CreateConstant({name});");
						//continue;
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

				var condition = GetConditionText(node.Condition);

				condition = condition != null ? $"ConditionCode.{condition}, " : string.Empty;

				if (!string.IsNullOrWhiteSpace(node.InstructionName))
				{
					var instruction = node.InstructionName.Replace("IR.", "IRInstruction."); ;
					var result = node == transform.ResultInstructionTree ? "result" : $"v{nodeNbrToVirtualRegisterNbr[node.NodeNbr]}";

					Lines.AppendLine($"\t\t\tcontext.{operation}Instruction({instruction}, {condition}{result}, {operands});");
				}
				else
				{
					Lines.AppendLine($"\t\t\tcontext.{operation}Instruction(GetMove(result), {condition}result, {operands});");
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
				if (operand.Value.Contains("0x") || operand.Value.Contains("0b"))
					return $"{operand.Value}";

				return $"{operand.Integer}";
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
				else if (parameter.IsInteger)
				{
					sb.Append(CreateConstantName(parameter));

					//sb.Append(parameter.Integer.ToString());
				}
				else if (parameter.IsDouble)
				{
					sb.Append(CreateConstantName(parameter));

					//sb.Append(parameter.Double.ToString());
				}
				else if (parameter.IsFloat)
				{
					sb.Append(CreateConstantName(parameter));

					//sb.Append(parameter.Float.ToString());
					//sb.Append('f');
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

			var path = NodeNbrToNode[instructionNode.NodeNbr];

			if (instructionNode.InstructionName.StartsWith("IR.Phi"))
			{
				EmitCondition($"context.{path}OperandCount != {instructionNode.Operands.Count}");
			}

			EmitConditions(instructionNode, path);

			foreach (var operand in instructionNode.Operands)
			{
				ProcessConditions(operand, instructionNode);
			}

			foreach (var operand in instructionNode.Operands)
			{
				ProcessNestedConditions(operand, instructionNode);
			}
		}

		private void EmitConditions(InstructionNode node, string path)
		{
			if (node.Condition == ConditionCode.Always)
				return;

			if (node.Conditions.Count == 1)
			{
				EmitCondition($"context.{path}ConditionCode != ConditionCode.{GetConditionText(node.Condition)}");
			}
			else
			{
				EmitConditionStatement(path);

				var sb = new StringBuilder();

				bool after = false;
				foreach (var condition in node.Conditions)
				{
					if (after)
						sb.Append(" || ");

					sb.Append($"context.{path}ConditionCode == ConditionCode.{GetConditionText(condition)}");

					after = true;
				}

				EmitCondition($"!({sb})");
			}
		}

		private static string GetConditionText(ConditionCode condition)
		{
			switch (condition)
			{
				case ConditionCode.Equal: return "Equal";
				case ConditionCode.NotEqual: return "NotEqual";
				case ConditionCode.Less: return "Less";
				case ConditionCode.LessOrEqual: return "LessOrEqual";
				case ConditionCode.Greater: return "Greater";
				case ConditionCode.GreaterOrEqual: return "GreaterOrEqual";

				case ConditionCode.UnsignedLess: return "UnsignedLess";
				case ConditionCode.UnsignedLessOrEqual: return "UnsignedLessOrEqual";
				case ConditionCode.UnsignedGreater: return "UnsignedGreater";
				case ConditionCode.UnsignedGreaterOrEqual: return "UnsignedGreaterOrEqual";

				default: return null;
			}
		}

		private void EmitConditionStatement(string path)
		{
			Lines.AppendLine($"\t\t\tvar condition = context.{path}ConditionCode;");
			Lines.AppendLine("");
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

			if (operand.IsDouble || operand.IsFloat || operand.IsInteger)
			{
				EmitCondition($"!context.{operandName}.IsResolvedConstant");
			}

			if (operand.IsInteger)
			{
				EmitCondition($"context.{operandName}.ConstantUnsigned64 != {CreateConstantName(operand)}");
			}

			if (operand.IsDouble)
			{
				EmitCondition($"context.{operandName}.ConstantDouble != {CreateConstantName(operand)}");
			}

			if (operand.IsFloat)
			{
				EmitCondition($"context.{operandName}.ConstantFloat != {CreateConstantName(operand)}");
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
