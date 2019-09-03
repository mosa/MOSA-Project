// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.Expression
{
	public class ExpressionOperand
	{
		public Token Token { get; set; }

		public string Value { get { return Token.Value; } }

		public int Index { get; }

		public ExpressionNode Node { get; set; }

		public ExpressionMethodFilter Method { get; set; }

		public bool IsExpressionNode { get { return Node != null; } }

		public bool IsMethod { get { return Method != null; } }

		public bool IsLabel { get { return (!IsExpressionNode && Token.TokenType == TokenType.Label) || (!IsExpressionNode && Token.TokenType == TokenType.Word); } }

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

		public string Label { get { return Token.Value; } }

		public ExpressionOperand(Token token, int index)
		{
			Token = token;
			Index = index;
		}

		public ExpressionOperand(ExpressionNode node, int index)
		{
			Node = node;
			Index = index;
		}

		public ExpressionOperand(ExpressionMethodFilter method, int index)
		{
			Method = method;
			Index = index;
		}

		public override string ToString()
		{
			if (Node != null)
				return $"{Index}:{Node}";
			else
				return $"{Index}:{Token}";
		}
	}
}
