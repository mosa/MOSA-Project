// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class SubByZero : BaseTransformation
	{
		private static List<BaseInstruction> SubInstructions = new List<BaseInstruction>() { IRInstruction.Sub32, IRInstruction.Sub64, IRInstruction.SubFloatR4, IRInstruction.SubFloatR8 };

		public SubByZero() : base(SubInstructions, OperandFilter.Any, OperandFilter.Constant_0)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(GetMove(context.Result), context.Result, context.Operand2);
		}
	}
}
