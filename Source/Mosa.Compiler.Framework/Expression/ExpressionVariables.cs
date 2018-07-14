// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Expression
{
	public class ExpressionVariables
	{
		public Dictionary<string, Operand> OperandMap = new Dictionary<string, Operand>();
		public Dictionary<string, MosaType> TypeMap = new Dictionary<string, MosaType>();

		public ExpressionVariables()
		{
		}

		public void Clear()
		{
			OperandMap.Clear();
			TypeMap.Clear();
		}

		public void SetOperand(string alias, Operand operand)
		{
			OperandMap.Add(alias, operand);
		}

		public void SetType(string alias, MosaType type)
		{
			TypeMap.Add(alias, type);
		}

		public Operand GetOperand(string alias)
		{
			OperandMap.TryGetValue(alias, out Operand operand);
			return operand;
		}

		public MosaType GetType(string alias)
		{
			TypeMap.TryGetValue(alias, out MosaType type);
			return type;
		}

		public bool ContainsOperand(string alias)
		{
			return OperandMap.ContainsKey(alias);
		}

		public bool ContainsType(string alias)
		{
			return TypeMap.ContainsKey(alias);
		}
	}
}
