using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.Compound;

/// <summary>
/// LoadCompound
/// </summary>
public sealed class LoadCompound : BaseTransform
{
	public LoadCompound() : base(IRInstruction.LoadCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CompoundHelper.Copy(transform, context, context.Result.Type, transform.StackFrame, context.Result, context.Operand1, context.Operand2);
	}
}
