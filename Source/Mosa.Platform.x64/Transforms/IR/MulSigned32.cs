// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR;

/// <summary>
/// MulSigned32
/// </summary>
public sealed class MulSigned32 : BaseIRTransform
{
	public MulSigned32() : base(IRInstruction.MulSigned32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var v1 = transform.AllocateVirtualRegister32();
		context.SetInstruction2(X64.Mul32, v1, context.Result, context.Operand1, context.Operand2);
	}
}
