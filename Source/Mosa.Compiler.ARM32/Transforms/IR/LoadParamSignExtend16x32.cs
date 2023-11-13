// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// LoadParamSignExtend16x32
/// </summary>
[Transform("ARM32.IR")]
public sealed class LoadParamSignExtend16x32 : BaseIRTransform
{
	public LoadParamSignExtend16x32() : base(Framework.IR.LoadParamSignExtend16x32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(!context.Result.IsR4);
		Debug.Assert(!context.Result.IsR8);

		TransformLoad(transform, context, ARM32.LdrS16, context.Result, transform.StackFrame, context.Operand1);
	}
}
