// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamSignExtend8x32
/// </summary>
public sealed class LoadParamSignExtend8x32 : BaseCodeMotionTransform
{
	public LoadParamSignExtend8x32() : base(Framework.IR.LoadParamSignExtend8x32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
