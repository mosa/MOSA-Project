// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

/// <summary>
/// GetLow32CPURegister
/// </summary>
public sealed class GetLow32CPURegister : BaseTransform
{
	public GetLow32CPURegister() : base(IRInstruction.GetLow32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!transform.Is32BitPlatform)
			return false;

		if (!transform.IsLowerTo32)
			return false;

		if (!context.Operand1.IsCPURegister)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		context.SetInstruction(IRInstruction.Move32, context.Result, context.Operand1);
	}
}
