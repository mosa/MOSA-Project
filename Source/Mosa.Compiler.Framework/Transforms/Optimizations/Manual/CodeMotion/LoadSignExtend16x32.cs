// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadSignExtend16x32
/// </summary>
public sealed class LoadSignExtend16x32 : BaseCodeMotionTransform
{
	public LoadSignExtend16x32() : base(IR.LoadSignExtend16x32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
