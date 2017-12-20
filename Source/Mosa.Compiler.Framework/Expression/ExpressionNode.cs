// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public class ExpressionNode
	{
		public TokenType Token { get; protected set; }

		public ExpressionNode Left { get; protected set; }
		public ExpressionNode Right { get; protected set; }

		public List<ExpressionNode> Parameters { get; protected set; }

		public ExpressionNode(TokenType token)
		{
			Token = token;
		}

		public ExpressionNode(TokenType token, ExpressionNode left)
		{
			Token = token;
			Left = left;
		}

		public ExpressionNode(TokenType token, ExpressionNode left, ExpressionNode right)
		{
			Token = token;
			Left = left;
			Right = right;
		}

		public ExpressionNode(TokenType token, List<ExpressionNode> parameters)
		{
			Token = token;
			Parameters = parameters;
		}

		public override string ToString()
		{
			return Token.ToString();
		}
	}
}
