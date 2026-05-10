// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamCompound
/// </summary>
public sealed class LoadParamCompound : BaseCodeMotionTransform
{
	public static readonly LoadParamCompound Instance = new();

	private LoadParamCompound() : base(IR.LoadParamCompound, TransformType.Manual | TransformType.Optimization)
	{
	}
}
