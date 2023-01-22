// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// BitCopyR4To32
	/// </summary>
	public sealed class BitCopyR4To32 : BaseTransform
	{
		public BitCopyR4To32() : base(IRInstruction.BitCopyR4To32, TransformType.Manual | TransformType.Transform)
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

			operand1 = X86TransformHelper.MoveConstantToFloatRegister(transform, context, operand1);

			context.SetInstruction(X86.Movdssi32, result, operand1);
		}
	}
}
