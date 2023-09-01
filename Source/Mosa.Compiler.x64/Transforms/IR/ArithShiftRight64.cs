// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// ArithShiftRight64
/// </summary>
[Transform("x64.IR")]
public sealed class ArithShiftRight64 : BaseIRTransform
{
	public ArithShiftRight64() : base(IRInstruction.ArithShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Sar64);
	}
}
