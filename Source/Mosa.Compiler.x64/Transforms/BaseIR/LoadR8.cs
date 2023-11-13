// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR;

/// <summary>
/// LoadR8
/// </summary>
[Transform("x64.BaseIR")]
public sealed class LoadR8 : BaseIRTransform
{
	public LoadR8() : base(Framework.IR.LoadR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR8);

		context.SetInstruction(X64.MovsdLoad, context.Result, context.Operand1, context.Operand2);
	}
}
