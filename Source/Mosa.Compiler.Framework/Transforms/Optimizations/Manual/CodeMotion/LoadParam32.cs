// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// Param32
/// </summary>
public sealed class LoadParam32 : BaseCodeMotionTransform
{
	public LoadParam32() : base(Framework.IR.LoadParam32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
