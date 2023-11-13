// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParam64
/// </summary>
public sealed class LoadParam64 : BaseCodeMotionTransform
{
	public LoadParam64() : base(IR.LoadParam64, TransformType.Manual | TransformType.Optimization)
	{
	}
}
