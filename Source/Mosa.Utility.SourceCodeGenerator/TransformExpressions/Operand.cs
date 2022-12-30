// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public class Operand
	{
		public Token Token { get; set; }

		public int Index { get; set; }

		public InstructionNode InstructionNode { get; set; }

		public Method Method { get; set; }

		public string Value { get { return Token.Value; } }

		public bool IsInstruction { get { return InstructionNode != null; } }

		public bool IsMethod { get { return Method != null; } }

		public bool IsLabel { get { return Token != null && Token.TokenType == TokenType.Label; } }

		public bool IsVirtualRegister { get { return IsInstruction; } }

		public bool IsAt { get { return Token != null && Token.TokenType == TokenType.At; } }

		public bool IsPercent { get { return Token != null && Token.TokenType == TokenType.Percent; } }

		public bool IsInteger { get { return Token != null && Token.TokenType == TokenType.IntegerConstant; } }

		public bool IsDouble { get { return Token != null && Token.TokenType == TokenType.DoubleConstant; } }

		public bool IsFloat { get { return Token != null && Token.TokenType == TokenType.FloatConstant; } }

		public bool IsAny { get { return Token != null && Token.TokenType == TokenType.Underscore; } }

		public ulong Integer { get { return Token.Integer; } }

		public double Double { get { return Token.Double; } }

		public double Float { get { return Token.Float; } }

		public string LabelName { get { return Token.Value; } }

		public Operand(Token token, int index)
		{
			Token = token;
			Index = index;
		}

		public Operand(InstructionNode instructionNode, int index)
		{
			InstructionNode = instructionNode;
			Index = index;
		}

		public Operand(Method method, int index)
		{
			Method = method;
			Index = index;
		}

		public Operand Clone(InstructionNode parent)
		{
			var operand = new Operand(Token, Index);

			if (InstructionNode != null)
			{
				operand.InstructionNode = InstructionNode.Clone(null);
			}

			if (Method != null)
			{
				operand.Method = Method;    // TODO - Okay for now since filters and results are not modified by variations
			}

			return operand;
		}

		public bool IsSame(Operand other)
		{
			if (IsLabel && other.IsLabel && LabelName == other.LabelName)
				return true;

			if (IsInteger && other.IsInteger && Integer == other.Integer)
				return true;

			if (IsFloat && other.IsFloat && Float == other.Float)
				return true;

			if (IsDouble && other.IsDouble && Double == other.Double)
				return true;

			return false;
		}

		public override string ToString()
		{
			if (IsInstruction)
				return $"{Index} : {InstructionNode}";
			else if (IsMethod)
				return $"{Index} : {Method}";
			else
				return $"{Index} : {Token}";
		}
	}
}
