// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.Simplification
{
	/// <summary>
	/// GetLow32FromShiftedMore32
	/// </summary>
	public sealed class GetLow32FromShiftedMore32 : BaseTransformation
	{
		public GetLow32FromShiftedMore32() : base(IRInstruction.GetLow32, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.ShiftLeft64)
				return false;

			if (!IsGreaterOrEqual(And32(To32(context.Operand1.Definitions[0].Operand2), 63), 32))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var c1 = transformContext.CreateConstant(0);

			context.SetInstruction(IRInstruction.Move32, result, c1);
		}
	}
}
