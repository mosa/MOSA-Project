using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Compound;

/// <summary>
/// LoadParamCompound
/// </summary>
public sealed class LoadParamCompound : BaseTransform
{
	public LoadParamCompound() : base(IRInstruction.LoadParamCompound, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		CompoundHelper.Copy(transform, context, context.Result.Type, transform.StackFrame, context.Result, transform.StackFrame, context.Operand1);
	}
}
