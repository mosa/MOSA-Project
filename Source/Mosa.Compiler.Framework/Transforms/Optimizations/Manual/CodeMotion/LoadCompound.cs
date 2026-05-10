// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadCompound
/// </summary>
public sealed class LoadCompound : BaseCodeMotionTransform
{
	public static readonly LoadCompound Instance = new();

	private LoadCompound() : base(IR.LoadCompound, TransformType.Manual | TransformType.Optimization)
	{
	}
}
