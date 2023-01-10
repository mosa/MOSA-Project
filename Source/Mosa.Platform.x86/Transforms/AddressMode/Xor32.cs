// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.AddressMode
{
	/// <summary>
	/// Xor32
	/// </summary>
	public sealed class Xor32 : BaseTransform
	{
		public Xor32() : base(X86.Xor32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == context.Operand1.Register)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			X86TransformHelper.AddressModeConversion(transform, context, X86.Mov32);
		}
	}
}
