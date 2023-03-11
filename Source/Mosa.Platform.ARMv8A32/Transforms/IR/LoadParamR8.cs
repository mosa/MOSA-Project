// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// LoadParamR8
/// </summary>
public sealed class LoadParamR8 : BaseIRTransform
{
	public LoadParamR8() : base(IRInstruction.LoadParamR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(context.Operand1.IsConstant);

		TransformLoad(transform, context, ARMv8A32.Ldf, context.Result, transform.StackFrame, context.Operand1);
	}
}
