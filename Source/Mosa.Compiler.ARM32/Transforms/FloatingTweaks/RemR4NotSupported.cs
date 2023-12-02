// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.FloatingTweaks;

/// <summary>
/// RemR4NotSupported
/// </summary>
public sealed class RemR4NotSupported : BaseTransform
{
	public RemR4NotSupported() : base(IR.RemR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		context.SetInstruction(IR.MoveR4, result, operand1); // NOT SUPPORTED
	}
}
