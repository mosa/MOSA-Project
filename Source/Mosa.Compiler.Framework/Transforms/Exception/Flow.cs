// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Exception;

/// <summary>
/// Flow
/// </summary>
public sealed class Flow : BaseExceptionTransform
{
	public Flow() : base(Framework.IR.Flow, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		context.Empty();
	}
}
