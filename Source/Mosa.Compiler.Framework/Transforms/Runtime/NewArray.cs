// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Runtime;

/// <summary>
/// NewArray
/// </summary>
public sealed class NewArray : BaseRuntimeTransform
{
	public NewArray() : base(IR.NewArray, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => -10;

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var method = GetVMCallMethod(transform, "AllocateArray");
		var symbol = Operand.CreateLabel(method, transform.Is32BitPlatform);

		context.SetInstruction(IR.CallStatic, context.Result, symbol, context.GetOperands());
	}
}
