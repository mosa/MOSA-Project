// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// LoadParamSignExtend8x32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class LoadParamSignExtend8x32 : BaseIRTransform
{
	public LoadParamSignExtend8x32() : base(IRInstruction.LoadParamSignExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		TransformLoad(transform, context, ARMv8A32.LdrS8, context.Result, transform.StackFrame, context.Operand1);
	}
}
