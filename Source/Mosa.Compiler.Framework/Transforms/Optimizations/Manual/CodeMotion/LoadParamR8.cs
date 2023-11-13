// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamR8
/// </summary>
public sealed class LoadParamR8 : BaseCodeMotionTransform
{
	public LoadParamR8() : base(Framework.IR.LoadParamR8, TransformType.Manual | TransformType.Optimization)
	{
	}
}
