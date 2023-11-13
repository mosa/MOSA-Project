// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadObject
/// </summary>
public sealed class LoadObject : BaseCodeMotionTransform
{
	public LoadObject() : base(Framework.IR.LoadObject, TransformType.Manual | TransformType.Optimization)
	{
	}
}
