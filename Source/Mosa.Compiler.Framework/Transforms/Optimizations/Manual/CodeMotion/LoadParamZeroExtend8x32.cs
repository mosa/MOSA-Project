// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamZeroExtend8x32
/// </summary>
public sealed class LoadParamZeroExtend8x32 : BaseCodeMotionTransform
{
	public LoadParamZeroExtend8x32() : base(IRInstruction.LoadParamZeroExtend8x32, TransformType.Manual | TransformType.Optimization)
	{
	}
}
