// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.SourceCodeGenerator.TransformExpressions
{
	public class InstructionNode
	{
		public string InstructionName { get; set; }

		public List<Operand> Operands { get; } = new List<Operand>();

		public int NodeNbr { get; set; }
	}
}
