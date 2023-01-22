// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// ConvertR8ToI32
	/// </summary>
	public sealed class ConvertR8ToI32 : BaseTransform
	{
		public ConvertR8ToI32() : base(IRInstruction.ConvertR8ToI32, TransformType.Manual | TransformType.Transform)
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

			context.SetInstruction(X86.Cvttsd2si32, result, operand1);
		}
	}
}
