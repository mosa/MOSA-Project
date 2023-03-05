// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.VM;

/// <summary>
/// NewArray
/// </summary>
public sealed class NewArray : BaseTransform
{
	public NewArray() : base(IRInstruction.NewArray, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var method = VMHelper.GetVMCallMethod(transform, "AllocateArray");
		var symbol = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);
		var arrayType = context.MosaType;

		context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.GetOperands());

		transform.MethodScanner.TypeAllocated(arrayType, method);
	}
}
