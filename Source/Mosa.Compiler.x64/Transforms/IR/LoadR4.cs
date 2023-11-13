// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.IR;

/// <summary>
/// LoadR4
/// </summary>
[Transform("x64.IR")]
public sealed class LoadR4 : BaseIRTransform
{
	public LoadR4() : base(Framework.IR.LoadR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsR4);

		context.SetInstruction(X64.MovssLoad, context.Result, context.Operand1, context.Operand2);
	}
}
