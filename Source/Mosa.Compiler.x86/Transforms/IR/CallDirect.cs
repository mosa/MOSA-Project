// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// CallDirect
/// </summary>
[Transform("x86.IR")]
public sealed class CallDirect : BaseIRTransform
{
	public CallDirect() : base(Framework.IR.CallDirect, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.ReplaceInstruction(X86.Call);
	}
}
