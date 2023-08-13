// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARM32.Transforms.Tweak;

/// <summary>
/// Mov
/// </summary>
[Transform("ARM32.Tweak")]
public sealed class Mov : BaseTransform
{
	public Mov() : base(ARM32.Mov, TransformType.Manual | TransformType.Transform)
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
