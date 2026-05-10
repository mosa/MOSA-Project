// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadParamR4
/// </summary>
public sealed class LoadParamR4 : BaseIRTransform
{
	public static readonly LoadParamR4 Instance = new();

	private LoadParamR4() : base(IR.LoadParamR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR4);

		context.SetInstruction(X86.MovssLoad, context.Result, transform.StackFrame, context.Operand1);
	}
}
