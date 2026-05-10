// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadR4
/// </summary>
public sealed class LoadR4 : BaseCodeMotionTransform
{
	public static readonly LoadR4 Instance = new();

	private LoadR4() : base(IR.LoadR4, TransformType.Manual | TransformType.Optimization)
	{
	}
}
