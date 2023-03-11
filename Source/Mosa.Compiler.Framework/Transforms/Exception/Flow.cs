namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// Flow
/// </summary>
public sealed class Flow : BaseExceptionTransform
{
	public Flow() : base(IRInstruction.Flow, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.Empty();
	}
}
