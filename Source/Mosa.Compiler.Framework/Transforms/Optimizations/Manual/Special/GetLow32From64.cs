// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

/// <summary>
/// GetLow32To64
/// </summary>
public sealed class GetLow32From64 : BaseTransform
{
	public GetLow32From64() : base(IRInstruction.GetLow32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!transform.Is32BitPlatform)
			return false;

		if (!transform.LowerTo32)
			return false;

		if (!context.Operand1.IsVirtualRegister)
			return false;

		//if (context.Operand1.Definitions.Count != 1)
		//	return false;

		if (context.Operand1.IsInteger32)
			return true;

		if (context.Operand1.IsReferenceType)
			return true;

		if (context.Operand1.IsPointer || context.Operand1.IsManagedPointer)
			return true;

		// TEMP
		if (context.Operand1.Type.IsValueType && MosaTypeLayout.IsPrimitive(context.Operand1.Type) && transform.TypeLayout.GetTypeSize(context.Operand1.Type) == 4)
			return true;

		return false;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.SetInstruction(IRInstruction.Move32, context.Result, context.Operand1);
	}
}
