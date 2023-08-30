// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.Tweak;

/// <summary>
/// Mvf
/// </summary>
[Transform("ARM32.Tweak")]
public sealed class Mvf : BaseTransform
{
	public Mvf() : base(ARM32.Mvf, TransformType.Manual | TransformType.Transform)
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
