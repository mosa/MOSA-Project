// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamZeroExtend8x32
/// </summary>
public sealed class LoadParamZeroExtend8x32 : BaseCodeMotionTransform
{
	public static readonly LoadParamZeroExtend8x32 Instance = new();

	private LoadParamZeroExtend8x32() : base(IR.LoadParamZeroExtend8x32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
