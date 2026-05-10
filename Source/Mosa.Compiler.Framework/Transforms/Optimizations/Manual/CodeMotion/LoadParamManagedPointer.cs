// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamObject
/// </summary>
public sealed class LoadParamManagedPointer : BaseCodeMotionTransform
{
	public static readonly LoadParamManagedPointer Instance = new();

	private LoadParamManagedPointer() : base(IR.LoadParamManagedPointer, TransformType.Manual | TransformType.Optimization)
	{
	}
}
