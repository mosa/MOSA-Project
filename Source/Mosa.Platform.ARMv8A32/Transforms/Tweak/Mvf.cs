// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.Tweak;

/// <summary>
/// Mvf
/// </summary>
[Transform("ARMv8A32.Tweak")]
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
