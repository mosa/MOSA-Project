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

			var reversePostOrder = ReversePostOrder(ResultInstructionTree);
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

		public List<InstructionNode> ReversePostOrder(InstructionNode tree)
		{
			int count = CountNodes(ResultInstructionTree);

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
	}
}
