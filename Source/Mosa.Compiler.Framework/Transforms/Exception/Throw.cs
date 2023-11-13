// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// Throw
/// </summary>
public sealed class Throw : BaseExceptionTransform
{
	public Throw() : base(Framework.IR.Throw, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var method = transform.Compiler.PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");

		context.SetInstruction(Framework.IR.MoveObject, transform.ExceptionRegister, context.Operand1);
		context.AppendInstruction(Framework.IR.CallStatic, null, Operand.CreateLabel(method, transform.Is32BitPlatform));

		transform.MethodScanner.MethodInvoked(method, transform.Method);
	}
}
