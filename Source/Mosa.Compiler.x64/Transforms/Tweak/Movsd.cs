// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Tweak;

/// <summary>
/// Movsd
/// </summary>
[Transform("x64.Tweak")]
public sealed class Movsd : BaseTransform
{
	public Movsd() : base(X64.Movsd, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return context.Operand1.IsPhysicalRegister && context.Result.Register == context.Operand1.Register;
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsPhysicalRegister);
		Debug.Assert(context.Operand1.IsPhysicalRegister);

		context.Empty();
	}
}
