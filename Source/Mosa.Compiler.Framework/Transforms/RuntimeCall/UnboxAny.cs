using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.RuntimeCall;

/// <summary>
/// UnboxAny
/// </summary>
public sealed class UnboxAny : BaseTransform
{
	public UnboxAny() : base(IRInstruction.UnboxAny, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		VMHelper.SetVMCall(transform, context, "UnboxAny", context.Result, context.GetOperands());
	}
}
