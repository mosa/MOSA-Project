// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// LoadParamSignExtend16x32
/// </summary>
public sealed class LoadParamSignExtend16x32 : BaseIRTransform
{
	public LoadParamSignExtend16x32() : base(IRInstruction.LoadParamSignExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(X86.MovsxLoad16, context.Result, transform.StackFrame, context.Operand1);
	}
}
