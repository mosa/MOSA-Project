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
		context.AppendInstruction(IRInstruction.CallStatic, null, Operand.CreateSymbolFromMethod(method, transform.TypeSystem));

		transform.MethodScanner.MethodInvoked(method, transform.Method);
	}
}
