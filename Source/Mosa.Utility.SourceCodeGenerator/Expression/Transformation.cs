// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Mosa.Utility.SourceCodeGenerator.Expression
{
	public class Transformation
	{
		public string Expression;
		public string Filter;
		public string Result;

		protected List<Token> TokenizedExpression;
		protected List<Token> TokenizedFilter;
		protected List<Token> TokenizedResult;

		public ExpressionNode ExpressionNode;

		public ExpressionLabels ExpressionLabels;

		public List<ExpressionMethodFilter> ExpressionMethodFilters;

		public Transformation(string expression, string filter, string result)
		{
			Expression = expression;
			Filter = filter;
			Result = result;

			TokenizedExpression = Tokenizer.Parse(Expression);
			TokenizedFilter = Tokenizer.Parse(Filter);
			TokenizedResult = Tokenizer.Parse(Result);

			ExpressionNode = ExpressionNode.Parse(TokenizedExpression);

			ExpressionLabels = new ExpressionLabels(ExpressionNode);

			ExpressionMethodFilters = ExpressionMethodFilter.ParseAll(TokenizedFilter);
		}

		public override string ToString()
		{
			return $"{Expression} & {Filter} -> {Result}";
		}
	}
}
