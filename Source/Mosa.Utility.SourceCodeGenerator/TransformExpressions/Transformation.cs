// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		}

		public override string ToString()
		{
			return $"{InstructionTree} & {FilterText} -> {ResultText}";
		}
	}
}
