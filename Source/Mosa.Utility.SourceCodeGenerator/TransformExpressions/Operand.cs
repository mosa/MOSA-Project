// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public class Operand
	{
		public Token Token { get; set; }

		public string Value { get { return Token.Value; } }

		public int Index { get; }

		public InstructionNode InstructionNode { get; set; }

		public Method Method { get; set; }

		public bool IsInstruction { get { return InstructionNode != null; } }

		public bool IsMethod { get { return Method != null; } }

		public bool IsLabel { get { return Token != null && Token.TokenType == TokenType.Label; } }

		public bool IsVirtualRegister { get { return IsInstruction; } }

		public bool IsLong { get { return Token != null && Token.TokenType == TokenType.LongConstant; } }

		public bool IsInteger { get { return Token != null && Token.TokenType == TokenType.IntegerConstant; } }

		public bool IsDouble { get { return Token != null && Token.TokenType == TokenType.DoubleConstant; } }

		public bool IsFloat { get { return Token != null && Token.TokenType == TokenType.FloatConstant; } }

		public bool IsAny { get { return Token != null && Token.TokenType == TokenType.Underscore; } }

		public ulong Long { get { return Token.Long; } }

		public uint Integer { get { return Token.Integer; } }

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
			this.InstructionNode = instructionNode;
			Index = index;
		}

		public Operand(Method method, int index)
		{
			Method = method;
			Index = index;
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
