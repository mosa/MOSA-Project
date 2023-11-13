// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// Load32
/// </summary>
public sealed class Load32 : BaseCodeMotionTransform
{
	public Load32() : base(Framework.IR.Load32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
