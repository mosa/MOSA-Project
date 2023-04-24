// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// Throw
/// </summary>
public sealed class Throw : BaseExceptionTransform
{
	public Throw() : base(IRInstruction.Throw, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var method = transform.Compiler.PlatformInternalRuntimeType.FindMethodByName("ExceptionHandler");

		context.SetInstruction(IRInstruction.MoveObject, transform.ExceptionRegister, context.Operand1);
		context.AppendInstruction(IRInstruction.CallStatic, null, Operand.CreateLabel(method, transform.Is32BitPlatform));

		transform.MethodScanner.MethodInvoked(method, transform.Method);
	}
}
