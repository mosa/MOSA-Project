// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Expression
{
	public class ExpressionVariables
	{
		public Operand[] Operands;
		public MosaType[] Types;

		public ExpressionVariables()
		{
			var operands = new Operand[4];
			var types = new MosaType[4];
		}

		public void Clear()
		{
			for (int i = 0; i < Operands.Length; i++)
				Operands[i] = null;

			for (int i = 0; i < Types.Length; i++)
				Types[i] = null;
		}
	}
}
