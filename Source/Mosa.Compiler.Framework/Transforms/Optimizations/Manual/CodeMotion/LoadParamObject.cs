// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamObject
/// </summary>
public sealed class LoadParamObject : BaseCodeMotionTransform
{
	public static readonly LoadParamObject Instance = new();

	private LoadParamObject() : base(IR.LoadParamObject, TransformType.Manual | TransformType.Optimization)
	{
	}
}
