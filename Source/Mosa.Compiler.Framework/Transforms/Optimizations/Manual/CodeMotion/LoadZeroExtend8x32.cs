// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadZeroExtend8x32
/// </summary>
public sealed class LoadZeroExtend8x32 : BaseCodeMotionTransform
{
	public static readonly LoadZeroExtend8x32 Instance = new();

	private LoadZeroExtend8x32() : base(IR.LoadZeroExtend8x32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
