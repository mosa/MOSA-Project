// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transformation.IR.ConstantFolding
{
	public sealed class ShiftZeroValue : BaseTransformation
	{
		private static List<BaseInstruction> ShiftInstructions = new List<BaseInstruction>() { IRInstruction.ShiftLeft32, IRInstruction.ShiftLeft64, IRInstruction.ShiftRight32, IRInstruction.ShiftRight64 };

		public ShiftZeroValue() : base(ShiftInstructions, OperandFilter.Constant_0, OperandFilter.Any)
		{
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.SetResultToZero(context);
		}
	}
}
