using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.RuntimeCall;

/// <summary>
/// Rethrow
/// </summary>
public sealed class Rethrow : BaseTransform
{
	public Rethrow() : base(IRInstruction.Rethrow, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		VMHelper.SetVMCall(transform, context, "Rethrow", context.Result, context.GetOperands());
	}
}
