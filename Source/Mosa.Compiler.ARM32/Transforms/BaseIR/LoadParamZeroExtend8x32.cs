// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// LoadParamZeroExtend8x32
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class LoadParamZeroExtend8x32 : BaseIRTransform
{
	public LoadParamZeroExtend8x32() : base(Framework.IR.LoadParamZeroExtend8x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		TransformLoad(transform, context, ARM32.Ldr8, context.Result, transform.StackFrame, context.Operand1);
	}
}
