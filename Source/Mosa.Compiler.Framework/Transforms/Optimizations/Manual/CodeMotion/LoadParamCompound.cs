// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamCompound
/// </summary>
public sealed class LoadParamCompound : BaseCodeMotionTransform
{
	public LoadParamCompound() : base(IRInstruction.LoadParamCompound, TransformType.Manual | TransformType.Optimization)
	{
	}
}
