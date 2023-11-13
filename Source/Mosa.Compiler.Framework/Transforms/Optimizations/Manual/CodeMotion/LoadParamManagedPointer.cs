// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamObject
/// </summary>
public sealed class LoadParamManagedPointer : BaseCodeMotionTransform
{
	public LoadParamManagedPointer() : base(Framework.IR.LoadParamManagedPointer, TransformType.Manual | TransformType.Optimization)
	{
	}
}
