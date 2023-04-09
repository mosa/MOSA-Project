// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// NewArray
/// </summary>
public sealed class NewArray : BaseRuntimeTransform
{
	public NewArray() : base(IRInstruction.NewArray, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var method = GetVMCallMethod(transform, "AllocateArray");
		var symbol = Operand.CreateSymbol(method, transform.Is32BitPlatform);
		var arrayType = context.MosaType;

		context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.GetOperands());

		transform.MethodScanner.TypeAllocated(arrayType, method);
	}
}
