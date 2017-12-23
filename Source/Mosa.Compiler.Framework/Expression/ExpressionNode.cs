// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Expression
{
	public class ExpressionNode
	{
		public Token Token { get; protected set; }

		public TokenType TokenType { get { return Token.TokenType; } }

		public ExpressionNode Left { get; protected set; }
		public ExpressionNode Right { get; protected set; }

		public List<ExpressionNode> Parameters { get; protected set; }

		public ExpressionNode(Token token)
		{
			Token = token;
		}

		public ExpressionNode(Token token, ExpressionNode left)
		{
			Token = token;
			Left = left;
		}

		public ExpressionNode(Token token, ExpressionNode left, ExpressionNode right)
		{
			Token = token;
			Left = left;
			Right = right;
		}

		public ExpressionNode(Token token, List<ExpressionNode> parameters)
		{
			Token = token;
			Parameters = parameters;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append(Token.ToString());

			if (Left != null && Right != null)
			{
				sb.Append("(");

				if (Left != null)
				{
					sb.Append(" ");
					sb.Append(Left.ToString());
				}

				if (Right != null)
				{
					sb.Append(" ");
					sb.Append(Right.ToString());
				}

				sb.Append(")");
			}

			return sb.ToString();
		}
	}
}
