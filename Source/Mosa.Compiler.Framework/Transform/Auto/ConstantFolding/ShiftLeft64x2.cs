// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transform.Auto.ConstantFolding
{
	/// <summary>
	/// ShiftLeft64x2
	/// </summary>
	public sealed class ShiftLeft64x2 : BaseTransformation
	{
		public ShiftLeft64x2() : base(IRInstruction.ShiftLeft64, TransformationType.Auto | TransformationType.Optimization)
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

			if (!IsResolvedConstant(context.Operand1.Definitions[0].Operand2))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			if (!IsIntegerBetween0And64(context.Operand1.Definitions[0].Operand2))
				return false;

			if (!IsIntegerBetween0And64(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand1.Definitions[0].Operand2;
			var t3 = context.Operand2;

			var e1 = transformContext.CreateConstant(Add64(To64(t2), To64(t3)));

			context.SetInstruction(IRInstruction.ShiftLeft64, result, t1, e1);
		}
	}
}
