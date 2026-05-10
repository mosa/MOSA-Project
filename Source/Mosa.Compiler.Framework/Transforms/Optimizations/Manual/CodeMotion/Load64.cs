// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// Load64
/// </summary>
public sealed class Load64 : BaseCodeMotionTransform
{
	public static readonly Load64 Instance = new();

	private Load64() : base(IR.Load64, TransformType.Manual | TransformType.Optimization)
	{
	}
}
