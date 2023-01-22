// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Movss
	/// </summary>
	public sealed class Movss : BaseTransform
	{
		public Movss() : base(X86.Movss, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return context.Operand1.IsCPURegister && context.Result.Register == context.Operand1.Register;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.Empty();
		}
	}
}
