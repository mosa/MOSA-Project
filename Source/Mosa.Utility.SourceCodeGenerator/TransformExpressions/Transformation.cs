// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
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

			LabelSet = new LabelSet(InstructionTree);

			Filters = FilterParser.ParseAll(TokenizedFilter);

			ResultInstructionTree = ResultParser.Parse(TokenizedResult);

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
							if (!(operand.IsInteger || operand.IsLong || operand.IsFloat || operand.IsDouble))
								result.Add(operand);
						}
					}
				}
			}

			return result;
		}
	}
}
