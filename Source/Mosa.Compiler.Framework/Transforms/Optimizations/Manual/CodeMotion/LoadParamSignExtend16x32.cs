// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamSignExtend16x32
/// </summary>
public sealed class LoadParamSignExtend16x32 : BaseCodeMotionTransform
{
	public static readonly LoadParamSignExtend16x32 Instance = new();

	private LoadParamSignExtend16x32() : base(IR.LoadParamSignExtend16x32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
