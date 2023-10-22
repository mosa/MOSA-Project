// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions;

public class Transformation
{
	public readonly string ExpressionText;
	public readonly string FilterText;
	public readonly string PrefilterText;
	public readonly string ResultText;

	protected readonly List<Token> TokenizedExpression;
	protected readonly List<Token> TokenizedFilter;
	protected readonly List<Token> TokenizedResult;
	protected readonly List<Token> TokenizedPrefilter;

	public readonly LabelSet LabelSet;
	public readonly Node InstructionTree;
	public readonly Node ResultInstructionTree;
	public readonly List<Method> Filters;
	public readonly List<Method> Prefilters;

	public Transformation(string expression, string filter, string result, string prefilterText)
	{
		ExpressionText = expression;
		FilterText = filter;
		ResultText = result;
		PrefilterText = prefilterText;

		TokenizedExpression = Tokenizer.Parse(ExpressionText);
		TokenizedFilter = Tokenizer.Parse(FilterText);
		TokenizedPrefilter = Tokenizer.Parse(PrefilterText);
		TokenizedResult = Tokenizer.Parse(ResultText);

		InstructionTree = InstructionParser.Parse(TokenizedExpression);
		ResultInstructionTree = ResultParser.Parse(TokenizedResult);
		Filters = FilterParser.ParseAll(TokenizedFilter);
		Prefilters = PrefilterParser.ParseAll(TokenizedPrefilter);

		LabelSet = new LabelSet(InstructionTree);
		LabelSet.AddUse(ResultInstructionTree);
	}

	private Transformation(Node instructionTree, Node resultInstructionTree, List<Method> filters, List<Method> prefilters)
	{
		InstructionTree = instructionTree;
		ResultInstructionTree = resultInstructionTree;
		Filters = filters;
		Prefilters = prefilters;

		LabelSet = new LabelSet(InstructionTree);
		LabelSet.AddUse(ResultInstructionTree);
	}

	public override string ToString()
	{
		return $"{InstructionTree} & {FilterText} -> {ResultText}";
	}

	public List<Node> __Preorder(Node tree)
	{
		var result = new List<Node>();
		var worklist = new Queue<Node>();
		var contains = new HashSet<Node>();

		worklist.Enqueue(tree);

		while (worklist.Count != 0)
		{
			var current = worklist.Dequeue();

			if (!contains.Contains(current))
			{
				result.Add(current);
				contains.Add(current);

				foreach (var next in current.Operands)
				{
					if (next.IsInstruction)
					{
						worklist.Enqueue(next.Node);
					}
				}
			}
		}

		return result;
	}

	public List<Node> GetPreorder(Node tree)
	{
		var result = new List<Node>();
		var worklist = new Stack<Node>();

		worklist.Push(tree);

		while (worklist.Count != 0)
		{
			var node = worklist.Pop();
			result.Add(node);

			foreach (var operand in node.Operands)
			{
				if (operand.IsInstruction)
				{
					worklist.Push(operand.Node);
				}
			}
		}

		return result;
	}

	public List<Method> GetPreorder(Method tree)
	{
		var result = new List<Method>();
		var worklist = new Stack<Method>();

		worklist.Push(tree);

		while (worklist.Count != 0)
		{
			var node = worklist.Pop();
			result.Add(node);

			foreach (var operand in node.Parameters)
			{
				if (operand.IsInstruction)
				{
					worklist.Push(operand.Method);
				}
			}
		}

		return result;
	}

	public List<Node> GetPostorder(Node tree)
	{
		var result = new List<Node>();
		var contains = new HashSet<Node>();

		var list = GetPreorder(tree);

		while (list.Count != result.Count)
		{
			foreach (var node in list)
			{
				if (contains.Contains(node))
					continue;

				var children = true;

				foreach (var operand in node.Operands)
				{
					if (operand.IsInstruction)
					{
						if (!contains.Contains(operand.Node))
						{
							children = false;
							break;
						}
					}
				}

				if (children)
				{
					result.Add(node);
					contains.Add(node);
					break;
				}
			}
		}

		return result;
	}

	public List<Method> GetPostorder(Method tree)
	{
		var result = new List<Method>();
		var contains = new HashSet<Method>();

		var list = GetPreorder(tree);

		while (list.Count != result.Count)
		{
			foreach (var node in list)
			{
				if (contains.Contains(node))
					continue;

				var children = true;

				foreach (var operand in node.Parameters)
				{
					if (operand.IsMethod)
					{
						if (!contains.Contains(operand.Method))
						{
							children = false;
							break;
						}
					}
				}

				if (children)
				{
					result.Add(node);
					contains.Add(node);
					break;
				}
			}
		}

		return result;
	}

	public List<Method> GetAllNodeMethods(Node tree)
	{
		var result = new List<Method>();
		var worklist = new Stack<Node>();

		worklist.Push(tree);

		while (worklist.Count != 0)
		{
			var node = worklist.Pop();

			foreach (var operand in node.Operands)
			{
				if (operand.IsInstruction)
				{
					worklist.Push(operand.Node);
				}
				if (operand.IsMethod)
				{
					result.Add(operand.Method);
				}
			}
		}

		return result;
	}

	public List<Operand> GetAllOperands(Node tree)
	{
		var result = new List<Operand>();
		var worklistNode = new Stack<Node>();
		var worklistMethod = new Stack<Method>();

		worklistNode.Push(tree);

		while (worklistNode.Count != 0 || worklistMethod.Count != 0)
		{
			while (worklistNode.Count != 0)
			{
				var node = worklistNode.Pop();

				foreach (var operand in node.Operands)
				{
					if (operand.IsInstruction)
					{
						worklistNode.Push(operand.Node);
					}
					else if (operand.IsMethod)
					{
						worklistMethod.Push(operand.Method);
					}
					else
					{
						result.Add(operand);
					}
				}
			}

			while (worklistMethod.Count != 0)
			{
				var node = worklistMethod.Pop();

				foreach (var operand in node.Parameters)
				{
					if (operand.IsMethod)
					{
						worklistMethod.Push(operand.Method);
					}
					else
					{
						// except constants in expressions
						if (!(operand.IsInteger || operand.IsFloat || operand.IsDouble))
							result.Add(operand);
					}
				}
			}
		}

		return result;
	}

	public List<Transformation> DeriveVariations(List<string> cumulativeInstructions)
	{
		var variations = new List<Transformation>();

		var bits = 0;

		foreach (var node in GetPreorder(InstructionTree))
		{
			if (cumulativeInstructions.Contains(node.InstructionName) && node.Operands.Count == 2 && node.Conditions.Count == 1)
			{
				if (!node.Operands[0].IsSame(node.Operands[1]))
				{
					bits++;
				}
			}
		}

		var total = 1 << bits;

		for (var index = 1; index < total; index++)
		{
			var variation = CreateVariation(cumulativeInstructions, index);

			if (variation != null)
				variations.Add(variation);
		}

		return variations;
	}

	private Transformation CreateVariation(List<string> cumulativeInstructions, int index)
	{
		var instructionTree = InstructionTree.Clone(null);

		var instructionNodes = GetPreorder(instructionTree);

		var bit = 0;

		foreach (var node in instructionNodes)
		{
			if (cumulativeInstructions.Contains(node.InstructionName) && node.Operands.Count == 2 && node.Conditions.Count == 1)
			{
				if (!node.Operands[0].IsSame(node.Operands[1]))
				{
					if (((index >> bit) & 1) == 1)
					{
						node.Condition = GetReverse(node.Condition);

						var temp = node.Operands[0];
						node.Operands[0] = node.Operands[1];
						node.Operands[1] = temp;

						node.Operands[0].Index = 0;
						node.Operands[1].Index = 1;
					}

					bit++;
				}
			}
		}

		return new Transformation(instructionTree, ResultInstructionTree, Filters, Prefilters);
	}

	public static ConditionCode GetReverse(ConditionCode conditionCode)
	{
		return conditionCode switch
		{
			ConditionCode.GreaterOrEqual => ConditionCode.LessOrEqual,
			ConditionCode.Greater => ConditionCode.LessOrEqual,
			ConditionCode.LessOrEqual => ConditionCode.GreaterOrEqual,
			ConditionCode.Less => ConditionCode.Greater,
			ConditionCode.UnsignedGreaterOrEqual => ConditionCode.UnsignedLessOrEqual,
			ConditionCode.UnsignedGreater => ConditionCode.UnsignedLess,
			ConditionCode.UnsignedLessOrEqual => ConditionCode.UnsignedGreaterOrEqual,
			ConditionCode.UnsignedLess => ConditionCode.UnsignedGreater,
			_ => conditionCode
		};
	}
}
