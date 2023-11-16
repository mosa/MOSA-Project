// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadR8
/// </summary>
public sealed class LoadR8 : BaseCodeMotionTransform
{
	public LoadR8() : base(IR.LoadR8, TransformType.Manual | TransformType.Optimization)
	{
	}
}
