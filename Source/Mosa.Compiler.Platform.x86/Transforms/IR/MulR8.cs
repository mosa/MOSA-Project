// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// MulR8
	/// </summary>
	public sealed class MulR8 : BaseTransform
	{
		public MulR8() : base(IRInstruction.MulR8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			operand1 = X86TransformHelper.MoveConstantToFloatRegister(transform, context, operand1);
			operand2 = X86TransformHelper.MoveConstantToFloatRegister(transform, context, operand2);

			context.SetInstruction(X86.Mulsd, result, operand1, operand2);
		}
	}
}
