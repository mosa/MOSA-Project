// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamZeroExtend16x32
/// </summary>
public sealed class LoadParamZeroExtend16x32 : BaseCodeMotionTransform
{
	public LoadParamZeroExtend16x32() : base(Framework.IR.LoadParamZeroExtend16x32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
