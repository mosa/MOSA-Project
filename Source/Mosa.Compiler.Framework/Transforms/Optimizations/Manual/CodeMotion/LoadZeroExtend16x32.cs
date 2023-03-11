// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadZeroExtend16x32
/// </summary>
public sealed class LoadZeroExtend16x32 : BaseCodeMotionTransform
{
	public LoadZeroExtend16x32() : base(IRInstruction.LoadZeroExtend16x32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
