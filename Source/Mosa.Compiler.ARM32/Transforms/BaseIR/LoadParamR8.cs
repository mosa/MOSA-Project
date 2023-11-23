// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// LoadParamR8
/// </summary>
public sealed class LoadParamR8 : BaseIRTransform
{
	public LoadParamR8() : base(IR.LoadParamR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Operand1.IsConstant);

		TransformFloatingPointLoad(transform, context, ARM32.Ldf, context.Result, transform.StackFrame, context.Operand1);
	}
}
