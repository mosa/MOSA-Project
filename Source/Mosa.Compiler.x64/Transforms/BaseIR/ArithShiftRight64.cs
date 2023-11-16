// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// ArithShiftRight64
/// </summary>
[Transform]
public sealed class ArithShiftRight64 : BaseIRTransform
{
	public ArithShiftRight64() : base(IR.ArithShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X64.Sar64);
	}
}
