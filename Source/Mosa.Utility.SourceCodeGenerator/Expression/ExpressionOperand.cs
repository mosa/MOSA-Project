// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.Expression
{
	public class ExpressionOperand
	{
		public Token Token { get; set; }

		public string Value { get { return Token.Value; } }

		public ExpressionNode Node { get; set; }

		public bool IsExpressionNode { get { return Node != null; } }

		public bool IsLabel { get { return !IsExpressionNode && Token.TokenType == TokenType.Word; } }

		public bool IsVirtualRegister { get { return IsExpressionNode; } }

		public bool IsLong { get { return !IsExpressionNode && Token.TokenType == TokenType.LongConstant; } }

		public bool IsInteger { get { return !IsExpressionNode && Token.TokenType == TokenType.IntegerConstant; } }

		public bool IsDouble { get { return !IsExpressionNode && Token.TokenType == TokenType.DoubleConstant; } }

		public bool IsFloat { get { return !IsExpressionNode && Token.TokenType == TokenType.FloatConstant; } }

		public bool IsAny { get { return !IsExpressionNode && Token.TokenType == TokenType.Underscore; } }

		public ulong Long { get { return Token.Long; } }

		public uint Integer { get { return Token.Integer; } }

		public double Double { get { return Token.Double; } }

		public double Float { get { return Token.Float; } }

		public ExpressionOperand(Token token)
		{
			Token = token;
		}

		public ExpressionOperand(ExpressionNode node)
		{
			Node = node;
		}

		public override string ToString()
		{
			if (Node != null)
				return Node.ToString();
			else
				return Token.ToString();
		}
	}
}
