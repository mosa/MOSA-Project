// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.IR
{
	/// <summary>
	/// MulUnsigned64
	/// </summary>
	public sealed class MulUnsigned64 : BaseTransform
	{
		public MulUnsigned64() : base(IRInstruction.MulUnsigned64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var v1 = transform.AllocateVirtualRegister32();
			context.SetInstruction2(X64.Mul64, v1, context.Result, context.Operand1, context.Operand2);
		}
	}
}
