// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadZeroExtend8x32
/// </summary>
public sealed class LoadZeroExtend8x32 : BaseCodeMotionTransform
{
	public LoadZeroExtend8x32() : base(IRInstruction.LoadZeroExtend8x32, TransformType.Manual | TransformType.Optimization, true)
	{
	}
}
