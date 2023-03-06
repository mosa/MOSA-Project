// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// LoadR8
/// </summary>
public sealed class LoadR8 : BaseIRTransform
{
	public LoadR8() : base(IRInstruction.LoadR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(context.Result.IsR8);

		context.SetInstruction(X86.MovsdLoad, context.Result, context.Operand1, context.Operand2);
	}
}
