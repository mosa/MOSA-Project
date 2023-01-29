// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions;

public class Operand
{
	public Token Token { get; set; }

	public int Index { get; set; }

	public InstructionNode InstructionNode { get; set; }

	public Method Method { get; set; }

	public string Value => Token.Value;

	public bool IsInstruction => InstructionNode != null;

	public bool IsMethod => Method != null;

	public bool IsLabel => Token != null && Token.TokenType == TokenType.Label;

	public bool IsVirtualRegister => IsInstruction;

	public bool IsAt => Token != null && Token.TokenType == TokenType.At;

	public bool IsPercent => Token != null && Token.TokenType == TokenType.Percent;

	public bool IsInteger => Token != null && Token.TokenType == TokenType.IntegerConstant;

	public bool IsDouble => Token != null && Token.TokenType == TokenType.DoubleConstant;

	public bool IsFloat => Token != null && Token.TokenType == TokenType.FloatConstant;

	public bool IsAny => Token != null && Token.TokenType == TokenType.Underscore;

	public ulong Integer => Token.Integer;

	public double Double => Token.Double;

	public double Float => Token.Float;

	public string LabelName => Token.Value;

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
