// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Transform.Manual.ConstantFolding
{
	/// <summary>
	/// Add32
	/// </summary>
	public sealed class AddCarryOut64 : BaseTransformation
	{
		public AddCarryOut64() : base(IRInstruction.AddCarryOut64, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;
			var result2 = context.Result2;

			var t1 = context.Operand1.ConstantUnsigned64;
			var t2 = context.Operand2.ConstantUnsigned64;

			var e1 = transformContext.CreateConstant(t1 + t2);
			var carry = IntegerTwiddling.IsAddOverflow(t1, t2);

			context.SetInstruction(IRInstruction.Move64, result, e1);
			context.AppendInstruction(IRInstruction.Move64, result2, carry ? transformContext.CreateConstant(1) : transformContext.ConstantZero64);
		}
	}
}
