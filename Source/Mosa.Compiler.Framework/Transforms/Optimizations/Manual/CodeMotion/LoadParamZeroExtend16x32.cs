// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamZeroExtend16x32
/// </summary>
public sealed class LoadParamZeroExtend16x32 : BaseCodeMotionTransform
{
	public static readonly LoadParamZeroExtend16x32 Instance = new();

	private LoadParamZeroExtend16x32() : base(IR.LoadParamZeroExtend16x32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
