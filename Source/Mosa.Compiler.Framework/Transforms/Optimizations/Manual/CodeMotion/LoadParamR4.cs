// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamR4
/// </summary>
public sealed class LoadParamR4 : BaseCodeMotionTransform
{
	public LoadParamR4() : base(IR.LoadParamR4, TransformType.Manual | TransformType.Optimization)
	{
	}
}
