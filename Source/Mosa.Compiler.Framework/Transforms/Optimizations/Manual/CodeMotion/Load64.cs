// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// Load64
/// </summary>
public sealed class Load64 : BaseCodeMotionTransform
{
	public Load64() : base(IR.Load64, TransformType.Manual | TransformType.Optimization)
	{
	}
}
