// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// ExceptionStart
/// </summary>
public sealed class ExceptionStart : BaseExceptionTransform
{
	public ExceptionStart() : base(IR.ExceptionStart, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var exceptionVirtualRegister = context.Result;

		context.SetInstruction(IR.KillAll);
		context.AppendInstruction(IR.Gen, transform.ExceptionRegister);
		context.AppendInstruction(IR.MoveObject, exceptionVirtualRegister, transform.ExceptionRegister);
	}
}
