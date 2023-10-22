// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// LoadParamR4
/// </summary>
[Transform("ARM32.IR")]
public sealed class LoadParamR4 : BaseIRTransform
{
	public LoadParamR4() : base(IRInstruction.LoadParamR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Operand1.IsConstant);

		TransformLoad(transform, context, ARM32.Ldf, context.Result, transform.StackFrame, context.Operand1);
	}
}
