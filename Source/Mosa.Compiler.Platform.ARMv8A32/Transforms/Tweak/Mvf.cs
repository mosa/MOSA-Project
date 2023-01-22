// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.Tweak
{
	/// <summary>
	/// Mvf
	/// </summary>
	public sealed class Mvf : BaseTransform
	{
		public Mvf() : base(ARMv8A32.Mvf, TransformType.Manual | TransformType.Transform)
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
