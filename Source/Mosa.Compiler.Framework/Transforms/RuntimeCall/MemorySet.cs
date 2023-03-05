using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.RuntimeCall;

/// <summary>
/// MemorySet
/// </summary>
public sealed class MemorySet : BaseTransform
{
	public MemorySet() : base(IRInstruction.MemorySet, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		VMHelper.SetVMCall(transform, context, "MemorySet", context.Result, context.GetOperands());
	}
}
