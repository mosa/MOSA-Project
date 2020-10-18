// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public class Transformation
	{
		public string ExpressionText;
		public string FilterText;
		public string ResultText;

		protected List<Token> TokenizedExpression;
		protected List<Token> TokenizedFilter;
		protected List<Token> TokenizedResult;

		public LabelSet LabelSet;
		public InstructionNode InstructionTree;
		public InstructionNode ResultInstructionTree;
		public List<Method> Filters;

		public Transformation(string expression, string filter, string result)
		{
			ExpressionText = expression;
			FilterText = filter;
			ResultText = result;

			TokenizedExpression = Tokenizer.Parse(ExpressionText);
			TokenizedFilter = Tokenizer.Parse(FilterText);
			TokenizedResult = Tokenizer.Parse(ResultText);

			InstructionTree = InstructionParser.Parse(TokenizedExpression);
			ResultInstructionTree = ResultParser.Parse(TokenizedResult);
			Filters = FilterParser.ParseAll(TokenizedFilter);

			LabelSet = new LabelSet(InstructionTree);
			LabelSet.AddUse(ResultInstructionTree);
		}

		private Transformation(InstructionNode instructionTree, InstructionNode resultInstructionTree, List<Method> filters)
		{
			InstructionTree = instructionTree;
			ResultInstructionTree = resultInstructionTree;
			Filters = filters;

			LabelSet = new LabelSet(InstructionTree);
			LabelSet.AddUse(ResultInstructionTree);
		}

		public override string ToString()
		{
			return $"{InstructionTree} & {FilterText} -> {ResultText}";
		}

		public List<InstructionNode> __Preorder(InstructionNode tree)
		{
			var result = new List<InstructionNode>();
			var worklist = new Queue<InstructionNode>();
			var contains = new HashSet<InstructionNode>();

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
							worklist.Enqueue(next.InstructionNode);
						}
					}
				}
			}

			return result;
		}

		public List<InstructionNode> GetPreorder(InstructionNode tree)
		{
			var result = new List<InstructionNode>();
			var worklist = new Stack<InstructionNode>();

			worklist.Push(tree);

			while (worklist.Count != 0)
			{
				var node = worklist.Pop();
				result.Add(node);

				foreach (var operand in node.Operands)
				{
					if (operand.IsInstruction)
					{
						worklist.Push(operand.InstructionNode);
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

		public List<InstructionNode> GetPostorder(InstructionNode tree)
		{
			var result = new List<InstructionNode>();
			var contains = new HashSet<InstructionNode>();

			var list = GetPreorder(tree);

			while (list.Count != result.Count)
			{
				foreach (var node in list)
				{
					if (contains.Contains(node))
						continue;

					bool children = true;

					foreach (var operand in node.Operands)
					{
						if (operand.IsInstruction)
						{
							if (!contains.Contains(operand.InstructionNode))
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

					bool children = true;

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

		public List<Method> GetAllInstructionNodeMethods(InstructionNode tree)
		{
			var result = new List<Method>();
			var worklist = new Stack<InstructionNode>();

			worklist.Push(tree);

			while (worklist.Count != 0)
			{
				var node = worklist.Pop();

				foreach (var operand in node.Operands)
				{
					if (operand.IsInstruction)
					{
						worklist.Push(operand.InstructionNode);
					}
					if (operand.IsMethod)
					{
						result.Add(operand.Method);
					}
				}
			}

			return result;
		}

		public List<Operand> GetAllOperands(InstructionNode tree)
		{
			var result = new List<Operand>();
			var worklistNode = new Stack<InstructionNode>();
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
							worklistNode.Push(operand.InstructionNode);
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

			int bits = 0;

			foreach (var node in GetPreorder(InstructionTree))
			{
				if (cumulativeInstructions.Contains(node.InstructionName) && node.Operands.Count == 2)
				{
					if (!node.Operands[0].IsSame(node.Operands[1]))
					{
						bits++;
					}
				}
			}

			int total = 1 << bits;

			for (int index = 1; index < total; index++)
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

			int bit = 0;

			foreach (var node in instructionNodes)
			{
				if (cumulativeInstructions.Contains(node.InstructionName) && node.Operands.Count == 2)
				{
					//if (node.InstructionName.Contains("Compare") && !(node.Condition == ConditionCode.Equal || node.Condition == ConditionCode.NotEqual))
					//	continue;

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

			return new Transformation(instructionTree, ResultInstructionTree, Filters);
		}

		public static ConditionCode GetReverse(ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case ConditionCode.GreaterOrEqual: return ConditionCode.LessOrEqual;
				case ConditionCode.Greater: return ConditionCode.LessOrEqual;
				case ConditionCode.LessOrEqual: return ConditionCode.GreaterOrEqual;
				case ConditionCode.Less: return ConditionCode.Greater;
				case ConditionCode.UnsignedGreaterOrEqual: return ConditionCode.UnsignedLessOrEqual;
				case ConditionCode.UnsignedGreater: return ConditionCode.UnsignedLess;
				case ConditionCode.UnsignedLessOrEqual: return ConditionCode.UnsignedGreaterOrEqual;
				case ConditionCode.UnsignedLess: return ConditionCode.UnsignedGreater;
				default: return conditionCode;
			}
		}
	}
}
