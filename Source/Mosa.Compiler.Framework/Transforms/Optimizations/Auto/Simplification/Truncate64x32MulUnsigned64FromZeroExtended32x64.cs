// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification
{
	/// <summary>
	/// Truncate64x32MulUnsigned64FromZeroExtended32x64
	/// </summary>
	public sealed class Truncate64x32MulUnsigned64FromZeroExtended32x64 : BaseTransform
	{
		public Truncate64x32MulUnsigned64FromZeroExtended32x64() : base(IRInstruction.Truncate64x32, TransformType.Auto | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulUnsigned64)
				return false;

			if (!context.Operand1.Definitions[0].Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand1.Definitions[0].Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand1.Definitions[0].Instruction != IRInstruction.ZeroExtend32x64)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Operand2.Definitions[0].Instruction != IRInstruction.ZeroExtend32x64)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

			context.SetInstruction(IRInstruction.MulUnsigned32, result, t1, t2);
		}
	}
}
