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

			var reversePostOrder = Preorder(ResultInstructionTree);
		}

		public override string ToString()
		{
			return $"{InstructionTree} & {FilterText} -> {ResultText}";
		}

		protected int CountNodes(InstructionNode node)
		{
			int count = 1;

			foreach (var operand in node.Operands)
			{
				if (operand.IsInstruction)
				{
					count += CountNodes(operand.InstructionNode);
				}
			}

			return count;
		}

		//protected int CountNodes(Method method)
		//{
		//	int count = 1;

		//	foreach (var operand in method.Parameters)
		//	{
		//		if (operand.IsInstruction)
		//		{
		//			count += CountNodes(operand.Method);
		//		}
		//	}

		//	return count;
		//}

		public List<InstructionNode> Preorder(InstructionNode tree)
		{
			int count = CountNodes(tree);

			var array = new BitArray(count, false);
			var result = new List<InstructionNode>();
			var worklist = new Queue<InstructionNode>();

			worklist.Enqueue(tree);

			while (worklist.Count != 0)
			{
				var current = worklist.Dequeue();

				if (!array.Get(current.NodeNbr))
				{
					result.Add(current);
					array.Set(current.NodeNbr, true);

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

		public List<InstructionNode> GetList(InstructionNode tree)
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

		public List<Method> GetList(Method tree)
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

		public List<InstructionNode> Postorder(InstructionNode tree)
		{
			// not written for performance! (but that's okay here)
			var result = new List<InstructionNode>();
			var constains = new HashSet<InstructionNode>();

			var list = GetList(tree);

			while (list.Count != result.Count)
			{
				foreach (var node in list)
				{
					if (constains.Contains(node))
						continue;

					bool children = true;

					foreach (var operand in node.Operands)
					{
						if (operand.IsInstruction)
						{
							if (!constains.Contains(operand.InstructionNode))
							{
								children = false;
								break;
							}
						}
					}

					if (children)
					{
						result.Add(node);
						constains.Add(node);
						break;
					}
				}
			}

			return result;
		}

		//public List<InstructionNode> Postorder(InstructionNode tree)
		//{
		//	// not written for performance! (but that's okay here)
		//	int count = CountNodes(tree);
		//	var result = new List<InstructionNode>(count);
		//	var array = new BitArray(count, false);

		//	var list = GetList(tree);

		//	while (count != result.Count)
		//	{
		//		foreach (var node in list)
		//		{
		//			if (array.Get(node.NodeNbr))
		//				continue;

		//			bool children = true;

		//			foreach (var operand in node.Operands)
		//			{
		//				if (operand.IsInstruction)
		//				{
		//					if (!array.Get(operand.InstructionNode.NodeNbr))
		//					{
		//						children = false;
		//						break;
		//					}
		//				}
		//			}

		//			if (children)
		//			{
		//				result.Add(node);
		//				array.Set(node.NodeNbr, true);
		//				break;
		//			}
		//		}
		//	}

		//	return result;
		//}

		public List<Method> Postorder(Method tree)
		{
			// not written for performance! (but that's okay here)
			var result = new List<Method>();
			var constains = new HashSet<Method>();

			var list = GetList(tree);

			while (list.Count != result.Count)
			{
				foreach (var node in list)
				{
					if (constains.Contains(node))
						continue;

					bool children = true;

					foreach (var operand in node.Parameters)
					{
						if (operand.IsMethod)
						{
							if (!constains.Contains(operand.Method))
							{
								children = false;
								break;
							}
						}
					}

					if (children)
					{
						result.Add(node);
						constains.Add(node);
						break;
					}
				}
			}

			return result;
		}
	}
}
