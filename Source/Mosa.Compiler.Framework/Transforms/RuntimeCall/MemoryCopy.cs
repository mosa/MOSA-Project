using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.RuntimeCall;

/// <summary>
/// MemoryCopy
/// </summary>
public sealed class MemoryCopy : BaseTransform
{
	public MemoryCopy() : base(IRInstruction.MemoryCopy, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		VMHelper.SetVMCall(transform, context, "MemoryCopy", context.Result, context.GetOperands());
	}
}
