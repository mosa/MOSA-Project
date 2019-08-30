// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class AddByZero2 : BaseTransformation
	{
		private static List<BaseInstruction> AddInstructions = new List<BaseInstruction>() { IRInstruction.Add32, IRInstruction.Add64, IRInstruction.AddFloatR4, IRInstruction.AddFloatR8 };

		public AddByZero2() : base(AddInstructions, OperandFilter.Constant_0, OperandFilter.Any)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			context.SetInstruction(GetMove(context.Result), context.Result, context.Operand2);
		}
	}
}
