// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// NewObject
/// </summary>
public sealed class NewObject : BaseRuntimeTransform
{
	public NewObject() : base(IRInstruction.NewObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var method = GetVMCallMethod(transform, "AllocateObject");
		var symbol = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);
		var classType = context.MosaType;

		context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.GetOperands());

		transform.MethodScanner.TypeAllocated(classType, transform.Method);
	}
}
