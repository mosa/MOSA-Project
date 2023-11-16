// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.Tweak;

/// <summary>
/// Mov
/// </summary>
[Transform]
public sealed class Mov : BaseTransform
{
	public Mov() : base(ARM32.Mov, TransformType.Manual | TransformType.Transform)
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
