// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// LoadR8
/// </summary>
public sealed class LoadR8 : BaseIRTransform
{
	public LoadR8() : base(IR.LoadR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR8);

		context.SetInstruction(X86.MovsdLoad, context.Result, context.Operand1, context.Operand2);
	}
}
