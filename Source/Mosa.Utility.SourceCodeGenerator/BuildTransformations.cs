// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;
using Mosa.Utility.SourceCodeGenerator.TransformExpressions;

namespace Mosa.Utility.SourceCodeGenerator;

public class BuildTransformations : BuildBaseTemplate
{
	public static readonly Dictionary<string, string> Transformations = new();

	protected readonly List<string> CommutativeInstructions = new();

	protected readonly Dictionary<int, string> NodeNbrToNode = new();

	protected readonly Dictionary<string, string> OperandLabelToVariable = new();

	protected bool First = true;

	protected readonly string Path;

	private string Family;
	private string Section;

	public BuildTransformations(string jsonFile, string destinationPath, string path)
		: base(jsonFile, destinationPath)
	{
		this.Path = path;
	}

	protected override void Iterator()
	{
		if (Entries.Commutative != null)
		{
			foreach (var instruction in Entries.Commutative)
			{
				CommutativeInstructions.Add(instruction);
			}
		}

		Family = Entries.Family;
		Section = Entries.Section;

		foreach (var entry in Entries.Optimizations)
		{
			Body(entry);
		}
	}

	protected override void Body(dynamic node = null)
	{
		string name = node.Name;
		string type = node.Type;
		string subName = node.SubName;
		string expression = node.Expression;
		string filter = node.Filter;
		string prefilter = node.Prefilter;
		string result = node.Result;
		string result2 = node.Result2;

		bool log = node.Log != null && node.Log == "Yes";
		bool variations = node.Variations != null && node.Variations == "Yes";

		bool optimization = node.Optimization != null && node.Optimization == "Yes";
		bool transformation = node.Transformation != null && node.Transformation == "Yes";
		bool ignore = node.Ignore != null && node.Ignore == "Yes";

		if (ignore)
			return;

		int priority = node.Priority != null ? int.Parse(node.Priority) : 0;

		if (!optimization && !transformation)
			optimization = true;

		List<string> commutativeInstructions = null;

		if (node.Commutative != null)
		{
			commutativeInstructions = new List<string>();
			foreach (var instruction in node.Commutative)
			{
				commutativeInstructions.Add(instruction);
			}
		}

		if (commutativeInstructions == null)
			commutativeInstructions = CommutativeInstructions;

		GenerateTranformations(name, type, subName, expression, filter, prefilter, result, variations, optimization, priority, log, commutativeInstructions, result2);
	}

	private void GenerateTranformations(string name, string type, string subName, string expression, string filter, string prefilter, string result, bool variations, bool optimization, int priority, bool log, List<string> commutativeInstructions, string result2)
	{
		if (expression.Contains("R#"))
		{
			GenerateTransformation(R4(name), R4(type), R4(subName), new Transformation(R4(expression), R4(filter), R4(result), R4(result2), prefilter), optimization, priority, variations, log, commutativeInstructions);
			GenerateTransformation(R8(name), R8(type), R8(subName), new Transformation(R8(expression), R8(filter), R8(result), R4(result2), prefilter), optimization, priority, variations, log, commutativeInstructions);
		}
		else if (expression.Contains("##"))
		{
			GenerateTransformation(To32(name), To32(type), To32(subName), new Transformation(To32(expression), To32(filter), To32(result), To32(result2), prefilter), optimization, priority, variations, log, commutativeInstructions);
			GenerateTransformation(To64(name), To64(type), To64(subName), new Transformation(To64(expression), To64(filter), To64(result), To32(result2), prefilter), optimization, priority, variations, log, commutativeInstructions);
		}
		else
		{
			GenerateTransformation(name, type, subName, new Transformation(expression, filter, result, result2, prefilter), optimization, priority, variations, log, commutativeInstructions);
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

	private void GenerateTransformation(string name, string type, string subName, Transformation transform, bool optimization, int priority, bool variations, bool log, List<string> commutativeInstructions)
	{
		Lines.Clear();
		First = true;

		DestinationFile = $"{type}/{name}{subName}.cs";
		AddSourceHeader();

		if (!Path.Contains("Framework"))
		{
			Lines.AppendLine($"using Mosa.Compiler.Framework;");
			Lines.AppendLine();
		}

		Lines.AppendLine($"namespace {Path}.{type};");
		Lines.AppendLine();

		GenerateTransformations(name, type, subName, transform, optimization, priority, variations, log, commutativeInstructions);

		Save();
	}

	private int CalculateLevel(Transformation transform)
	{
		var nodes = CountNodes(transform.InstructionTree);

		var windows = CountWindows(transform.Prefilters);
		var windows2 = CountWindows(transform.Filters);

		var cost = ((nodes - 1) * 10) + ((windows + windows2) * 25);

		cost = Math.Min(cost, 100);

		return 100 - cost;
	}

	private int CountNodes(Node parent)
	{
		var count = 1;

		foreach (var operand in parent.Operands)
		{
			if (operand.IsInstruction)
				count += CountNodes(operand.Node);
		}

		return count;
	}

	private int CountWindows(List<Method> methods)
	{
		var count = 0;

		foreach (var method in methods)
		{
			foreach (var parameter in method.Parameters)
			{
				if (parameter.IsDollar && parameter.LabelName == "Window")
					count++;
			}
		}

		return count;
	}

	private void GenerateTransformations(string name, string type, string subName, Transformation transform, bool optimization, int priority, bool variations, bool log, List<string> commutativeInstructions)
	{
		GenerateTransformation2(name, type, subName, transform, optimization, priority, log);

		if (!variations)
			return;

		if (commutativeInstructions == null || commutativeInstructions.Count == 0)
			return;

		var derivedVariations = transform.DeriveVariations(commutativeInstructions);

		var index = 1;
		foreach (var variation in derivedVariations)
		{
			GenerateTransformation2(name, type, $"{subName}_v{index}", variation, optimization, priority, log);
			index++;
		}
	}

	private void GenerateTransformation2(string name, string type, string subName, Transformation transform, bool optimization, int priority, bool log)
	{
		var instructionName = transform.InstructionTree.InstructionName;

		Transformations.Add($"{Path}.{type}.{name}{subName}", $"{type}.{name}{subName}");

		if (First)
		{
			First = false;
		}
		else
		{
			Lines.AppendLine();
		}

		Lines.AppendLine($"public sealed class {name}{subName} : BaseTransform");
		Lines.AppendLine("{");

		var typestring = $"TransformType.Auto{(optimization ? " | TransformType.Optimization" : string.Empty)}";

		Lines.Append($"\tpublic {name}{subName}() : base({instructionName}, {typestring}");

		if (priority != 0)
			Lines.Append($", {priority}");

		if (log)
			Lines.Append($", true");

		Lines.AppendLine(")");

		Lines.AppendLine("\t{");
		Lines.AppendLine("\t}");
		Lines.AppendLine("");

		//if (priority != 0)
		//{
		//	Lines.AppendLine($"\tpublic override int Priority => {priority};");
		//	Lines.AppendLine("");
		//}

		Lines.AppendLine("\tpublic override bool Match(Context context, Transform transform)");
		Lines.AppendLine("\t{");

		ProcessPrefilters(transform);

		ProcessExpressionNode(transform.InstructionTree);

		ProcessDuplicatesInExpression(transform);

		ProcessFilters(transform);

		Lines.AppendLine("\t\treturn true;");
		Lines.AppendLine("\t}");

		Lines.AppendLine("");
		Lines.AppendLine("\tpublic override void Transform(Context context, Transform transform)");
		Lines.AppendLine("\t{");

		ProcessResultInstructionTree(transform);

		Lines.AppendLine("\t}");
		Lines.AppendLine("}");

		NodeNbrToNode.Clear();
		OperandLabelToVariable.Clear();
	}

	private void ProcessResultInstructionTree(Transformation transform)
	{
		// Capture the result type
		Lines.AppendLine("\t\tvar result = context.Result;");

		if (transform.Result2InstructionTree != null)
		{
			Lines.AppendLine("\t\tvar result2 = context.Result2;");
		}

		Lines.AppendLine("");

		// Capture all the labeled operands into variables
		var labelCount = 0;
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

			Lines.AppendLine($"\t\tvar t{labelCount} = {labelName};");
		}
		if (labelCount != 0)
			Lines.AppendLine("");

		// Create virtual register for each child instruction
		var nodeNbrToVirtualRegisterNbr = new Dictionary<int, int>();

		var postOrder = CreateVirtualRegisters(transform, nodeNbrToVirtualRegisterNbr, transform.ResultInstructionTree);
		var postOrder2 = CreateVirtualRegisters(transform, nodeNbrToVirtualRegisterNbr, transform.Result2InstructionTree);

		if (nodeNbrToVirtualRegisterNbr.Count != 0)
			Lines.AppendLine();

		// Create all the constants variables
		var operandList = Transformation.GetAllOperands(transform.ResultInstructionTree, transform.Result2InstructionTree);
		var constantNbr = 0;
		var constantTextToConstantNbr = new Dictionary<string, int>();
		var constantToConstantNbr = new Dictionary<Operand, int>();
		foreach (var operand in operandList)
		{
			var name = CreateConstantName(operand);

			if (name == null)
				continue;

			if (constantTextToConstantNbr.TryGetValue(name, out var found))
			{
				constantToConstantNbr.Add(operand, found);
				Lines.AppendLine($"\t\tvar c{operand} = {CreateConstant(name)};");
				continue;
			}

			constantNbr++;
			constantTextToConstantNbr.Add(name, constantNbr);
			constantToConstantNbr.Add(operand, constantNbr);

			Lines.AppendLine($"\t\tvar c{constantNbr} = {CreateConstant(name)};");
		}
		if (constantNbr != 0)
			Lines.AppendLine("");

		// Evaluate functions
		var methodToExpressionText = new Dictionary<string, int>();
		var methodToMethodNbr = new Dictionary<Method, int>();

		EvaluateFunctions(labelToLabelNbr, constantToConstantNbr, methodToExpressionText, methodToMethodNbr, postOrder);
		EvaluateFunctions(labelToLabelNbr, constantToConstantNbr, methodToExpressionText, methodToMethodNbr, postOrder2);

		if (methodToMethodNbr.Count != 0)
			Lines.AppendLine("");

		// Create Instructions
		CreateInstructions(transform, labelToLabelNbr, nodeNbrToVirtualRegisterNbr, constantToConstantNbr, methodToMethodNbr, true, postOrder);
		CreateInstructions(transform, labelToLabelNbr, nodeNbrToVirtualRegisterNbr, constantToConstantNbr, methodToMethodNbr, false, postOrder2);
	}

	private void CreateInstructions(Transformation transform, Dictionary<string, int> labelToLabelNbr, Dictionary<int, int> nodeNbrToVirtualRegisterNbr, Dictionary<Operand, int> constantToConstantNbr, Dictionary<Method, int> methodToMethodNbr, bool firstInstruction, List<Node> postOrder)
	{
		if (postOrder == null)
			return;

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
					sb.Append($"v{nodeNbrToVirtualRegisterNbr[operand.Node.NodeNbr]}");
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
				var instruction = node.InstructionName;
				var result =
					node == transform.ResultInstructionTree ? "result"
						: node == transform.Result2InstructionTree ? "result2"
						: $"v{nodeNbrToVirtualRegisterNbr[node.NodeNbr]}";

				Lines.AppendLine($"\t\tcontext.{operation}Instruction({instruction}, {condition}{result}, {operands});");
			}
			else
			{
				Lines.AppendLine($"\t\tcontext.{operation}Instruction(GetMove(result), {condition}result, {operands});");
			}

			firstInstruction = false;
		}
	}

	private void EvaluateFunctions(Dictionary<string, int> labelToLabelNbr, Dictionary<Operand, int> constantToConstantNbr, Dictionary<string, int> methodToExpressionText, Dictionary<Method, int> methodToMethodNbr, List<Node> postOrder)
	{
		if (postOrder == null)
			return;

		foreach (var node in postOrder)
		{
			foreach (var operand in node.Operands)
			{
				if (!operand.IsMethod)
					continue;

				var name = CreateExpression(operand.Method, labelToLabelNbr, constantToConstantNbr);

				if (methodToExpressionText.TryGetValue(name, out var found))
				{
					methodToMethodNbr.Add(operand.Method, found);
					continue;
				}

				var methodNbr = methodToMethodNbr.Count + 1;

				methodToMethodNbr.Add(operand.Method, methodNbr);
				methodToExpressionText.Add(name, methodNbr);

				Lines.AppendLine($"\t\tvar e{methodNbr} = {CreateConstant(name)};");
			}
		}
	}

	private List<Node> CreateVirtualRegisters(Transformation transform, Dictionary<int, int> nodeNbrToVirtualRegisterNbr, Node resultTree)
	{
		if (resultTree == null)
			return null;

		var postOrder = transform.GetPostorder(resultTree);

		foreach (var node in postOrder)
		{
			if (node == resultTree)
				continue;

			var resultType = DetermineResultType(node);

			var virtualRegisterNbr = nodeNbrToVirtualRegisterNbr.Count + 1;

			nodeNbrToVirtualRegisterNbr.Add(node.NodeNbr, virtualRegisterNbr);

			Lines.AppendLine($"\t\tvar v{virtualRegisterNbr} = transform.VirtualRegisters.Allocate{resultType}();");
		}

		return postOrder;
	}

	private static string CreateConstant(string value) => $"Operand.CreateConstant({value})"
			.Replace("Operand.CreateConstant(To32(0))", "Operand.Constant32_0")
			.Replace("Operand.CreateConstant(To32(1))", "Operand.Constant32_1")
			.Replace("Operand.CreateConstant(To32(2))", "Operand.Constant32_2")
			.Replace("Operand.CreateConstant(To32(4))", "Operand.Constant32_4")
			.Replace("Operand.CreateConstant(To32(0xFFFFFFFF))", "Operand.Constant32_FFFFFFFF")
			.Replace("Operand.CreateConstant(To64(0))", "Operand.Constant64_0")
			.Replace("Operand.CreateConstant(To64(1))", "Operand.Constant64_1")
			.Replace("Operand.CreateConstant(To64(2))", "Operand.Constant64_2")
			.Replace("Operand.CreateConstant(To64(4))", "Operand.Constant64_4");

	private string CreateExpression(Method method, Dictionary<string, int> labelToLabelNbr, Dictionary<Operand, int> constantToConstantNbr)
	{
		var sb = new StringBuilder();

		sb.Append(method.MethodName);
		sb.Append('(');

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

		sb.Append(')');

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

	private string DetermineResultType(Node node)
	{
		if (!string.IsNullOrWhiteSpace(node.ResultType))
			return node.ResultType;

		if (node.InstructionName.EndsWith("32"))
			return "32";

		if (node.InstructionName.EndsWith("64"))
			return "64";

		if (node.InstructionName.EndsWith("R4"))
			return "R4";

		if (node.InstructionName.EndsWith("R8"))
			return "R8";

		if (node.InstructionName.EndsWith("Object"))
			return "Object";

		// TODO

		return "64";    // default
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

			for (var i = 1; i < label.Positions.Count; i++)
			{
				var other = label.Positions[i];
				var otherParent = NodeNbrToNode[other.NodeNbr];
				var otherOperandName = GetOperandName(other.OperandIndex);
				var otherName = $"context.{otherParent}{otherOperandName}";

				EmitCondition($"!AreSame({firstName}, {otherName})");
			}
		}
	}

	private void ProcessPrefilters(Transformation transform)
	{
		var filters = transform.Prefilters;

		foreach (var filter in filters)
		{
			var sb = new StringBuilder();

			if (!filter.IsNegated)
				sb.Append('!');

			sb.Append(ProcessPrefilters(filter));

			EmitCondition(sb.ToString());
		}
	}

	private string ProcessPrefilters(Method filter)
	{
		var sb = new StringBuilder();

		sb.Append("transform.");
		sb.Append(filter.MethodName);
		sb.Append('(');

		foreach (var parameter in filter.Parameters)
		{
			if (parameter.IsInteger)
			{
				sb.Append(CreateConstantName(parameter));
			}
			else if (parameter.IsDouble)
			{
				sb.Append(CreateConstantName(parameter));
			}
			else if (parameter.IsFloat)
			{
				sb.Append(CreateConstantName(parameter));
			}
			else if (parameter.IsDollar)
			{
				sb.Append($"transform.{parameter.Value}");
			}
			else if (parameter.IsAt)
			{
				sb.Length--;
				return sb.ToString();
			}

			sb.Append(", ");
		}

		if (filter.Parameters.Count != 0)
			sb.Length -= 2;

		sb.Append(')');

		return sb.ToString();
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

		var register = false;

		foreach (var parameter in filter.Parameters)
		{
			if (parameter.IsMethod)
			{
				sb.Append(ProcessFilters(parameter.Method, transform));
			}
			else if (parameter.IsLabel)
			{
				if (!register)
				{
					var first = transform.LabelSet.GetFirstPosition(parameter.Value);

					var parent = NodeNbrToNode[first.NodeNbr];

					var operandName = GetOperandName(first.OperandIndex);

					sb.Append($"context.{parent}{operandName}");
				}
				else
				{
					sb.Append($"{parameter.LabelName}");
					register = false;
				}
			}
			else if (parameter.IsAt)
			{
				sb.Append("context");
			}
			else if (parameter.IsDollar)
			{
				sb.Append($"transform.{parameter.Value}");
			}
			else if (parameter.IsPercent)
			{
				sb.Append("CPURegister.");
				register = true;
			}
			else if (parameter.IsInteger)
			{
				sb.Append(CreateConstantName(parameter));
			}
			else if (parameter.IsDouble)
			{
				sb.Append(CreateConstantName(parameter));
			}
			else if (parameter.IsFloat)
			{
				sb.Append(CreateConstantName(parameter));
			}

			if (!register)
			{
				sb.Append(", ");
			}
		}

		if (filter.Parameters.Count != 0)
			sb.Length -= 2;

		sb.Append(')');

		return sb.ToString();
	}

	private void ProcessExpressionNode(Node instructionNode)
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

	private void EmitConditions(Node node, string path)
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

			var first = true;
			foreach (var condition in node.Conditions)
			{
				if (first)
				{
					sb.Append($"context.{path}ConditionCode is ConditionCode.{GetConditionText(condition)}");
					first = false;
				}
				else
				{
					sb.Append($" or ConditionCode.{GetConditionText(condition)}");
				}
			}

			EmitCondition($"!({sb})");
		}
	}

	private static string GetConditionText(ConditionCode condition)
	{
		return condition switch
		{
			ConditionCode.Equal => "Equal",
			ConditionCode.NotEqual => "NotEqual",
			ConditionCode.Less => "Less",
			ConditionCode.LessOrEqual => "LessOrEqual",
			ConditionCode.Greater => "Greater",
			ConditionCode.GreaterOrEqual => "GreaterOrEqual",
			ConditionCode.UnsignedLess => "UnsignedLess",
			ConditionCode.UnsignedLessOrEqual => "UnsignedLessOrEqual",
			ConditionCode.UnsignedGreater => "UnsignedGreater",
			ConditionCode.UnsignedGreaterOrEqual => "UnsignedGreaterOrEqual",
			_ => null
		};
	}

	private void EmitConditionStatement(string path)
	{
		Lines.AppendLine($"\t\tvar condition = context.{path}ConditionCode;");
		Lines.AppendLine("");
	}

	private void EmitCondition(string condition)
	{
		Lines.AppendLine($"\t\tif ({condition})");
		Lines.AppendLine($"\t\t\treturn false;");
		Lines.AppendLine("");
	}

	protected void ProcessConditions(Operand operand, Node parent)
	{
		if (operand.IsAny)
			return; // nothing;

		var operandName = NodeNbrToNode[parent.NodeNbr] + GetOperandName(operand.Index);

		if (operand.IsInstruction)
		{
			EmitCondition($"!context.{operandName}.IsVirtualRegister");
		}

		if (operand.IsInteger && operand.Integer == 0)
		{
			EmitCondition($"!context.{operandName}.IsConstantZero");
			return;
		}

		if (operand.IsInteger && operand.Integer == 1)
		{
			EmitCondition($"!context.{operandName}.IsConstantOne");
			return;
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

	protected void ProcessNestedConditions(Operand operand, Node node)
	{
		if (!operand.IsInstruction)
			return;

		var parent = $"{NodeNbrToNode[node.NodeNbr]}";
		var operandName = GetOperandName(operand.Index);

		EmitCondition($"!context.{parent}{operandName}.IsDefinedOnce");

		var instructionName = operand.Node.InstructionName;

		EmitCondition($"context.{parent}{operandName}.Definitions[0].Instruction != {instructionName}");

		NodeNbrToNode.Add(operand.Node.NodeNbr, $"{parent}{operandName}.Definitions[0].");

		ProcessExpressionNode(operand.Node);
	}

	protected string GetOperandName(int index)
	{
		if (index < 5)
			return $"Operand{index + 1}";
		else
			return $"GetOperand({index})";
	}
}
