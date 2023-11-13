// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadR4
/// </summary>
[Transform("x86.BaseIR")]
public sealed class LoadR4 : BaseIRTransform
{
	public LoadR4() : base(Framework.IR.LoadR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR4);

		context.SetInstruction(X86.MovssLoad, context.Result, context.Operand1, context.Operand2);
	}
}
