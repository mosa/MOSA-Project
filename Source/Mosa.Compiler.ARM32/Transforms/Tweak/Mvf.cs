// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.Tweak;

/// <summary>
/// Mvf
/// </summary>
public sealed class Mvf : BaseTransform
{
	public Mvf() : base(ARM32.Mvf, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return context.Operand1.IsPhysicalRegister && context.Result.Register == context.Operand1.Register;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.Empty();
	}
}
