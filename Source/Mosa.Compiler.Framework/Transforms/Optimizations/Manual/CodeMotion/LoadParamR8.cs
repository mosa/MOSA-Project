// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamR8
/// </summary>
public sealed class LoadParamR8 : BaseCodeMotionTransform
{
	public static readonly LoadParamR8 Instance = new();

	private LoadParamR8() : base(IR.LoadParamR8, TransformType.Manual | TransformType.Optimization)
	{
	}
}
