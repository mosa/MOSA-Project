// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.Simplification
{
	/// <summary>
	/// Truncate64x32Xor64FromZeroExtended32x64
	/// </summary>
	public sealed class Truncate64x32Xor64FromZeroExtended32x64 : BaseTransformation
	{
		public Truncate64x32Xor64FromZeroExtended32x64() : base(IRInstruction.Truncate64x32, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.Xor64)
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

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2.Definitions[0].Operand1;

			context.SetInstruction(IRInstruction.Xor32, result, t1, t2);
		}
	}
}
