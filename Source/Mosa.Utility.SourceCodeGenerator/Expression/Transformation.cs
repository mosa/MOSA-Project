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

		public Transformation(string expression, string filter, string result)
		{
			Expression = expression;
			Filter = filter;
			Result = result;

			Parse();
		}

		protected void Parse()
		{
			TokenizedExpression = Tokenizer.Parse(Expression);
			TokenizedFilter = Tokenizer.Parse(Filter);
			TokenizedResult = Tokenizer.Parse(Result);

			ExpressionNode = ExpressionNode.Parse(TokenizedExpression);

			return;
		}

		public override string ToString()
		{
			return $"{Expression} & {Filter} -> {Result}";
		}
	}
}
