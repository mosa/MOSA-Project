// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// NewObject
/// </summary>
public sealed class NewObject : BaseRuntimeTransform
{
	public NewObject() : base(IR.NewObject, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var method = GetVMCallMethod(transform, "AllocateObject");
		var symbol = Operand.CreateLabel(method, transform.Is32BitPlatform);

		context.SetInstruction(IR.CallStatic, context.Result, symbol, context.GetOperands());
	}
}
