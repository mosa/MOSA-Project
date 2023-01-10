// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.AddressMode
{
	/// <summary>
	/// Mulss
	/// </summary>
	public sealed class Mulss : BaseTransform
	{
		public Mulss() : base(X86.Mulss, TransformType.Manual | TransformType.Transform)
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
			X86TransformHelper.AddressModeConversion(transform, context, X86.Movss);
		}
	}
}
