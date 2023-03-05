// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.VM;

/// <summary>
/// NewObject
/// </summary>
public sealed class NewObject : BaseTransform
{
	public NewObject() : base(IRInstruction.NewObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var method = VMHelper.GetVMCallMethod(transform, "AllocateObject");
		var symbol = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);
		var classType = context.MosaType;

		context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.GetOperands());

		transform.MethodScanner.TypeAllocated(classType, transform.Method);
	}
}
