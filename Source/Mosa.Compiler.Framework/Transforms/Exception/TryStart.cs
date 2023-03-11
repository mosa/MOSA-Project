namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// TryStart
/// </summary>
public sealed class TryStart : BaseExceptionTransform
{
	public TryStart() : base(IRInstruction.TryStart, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		context.Empty();
	}
}
