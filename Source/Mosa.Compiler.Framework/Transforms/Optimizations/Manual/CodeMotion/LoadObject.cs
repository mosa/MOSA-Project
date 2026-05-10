// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadObject
/// </summary>
public sealed class LoadObject : BaseCodeMotionTransform
{
	public static readonly LoadObject Instance = new();

	private LoadObject() : base(IR.LoadObject, TransformType.Manual | TransformType.Optimization)
	{
	}
}
