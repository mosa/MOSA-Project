// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// CallDirect
/// </summary>
[Transform("x64.IR")]
public sealed class CallDirect : BaseIRTransform
{
	public CallDirect() : base(IRInstruction.CallDirect, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.ReplaceInstruction(X64.Call);
	}
}
