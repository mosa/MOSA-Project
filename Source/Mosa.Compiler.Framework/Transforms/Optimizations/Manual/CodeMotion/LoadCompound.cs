// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadCompound
/// </summary>
public sealed class LoadCompound : BaseCodeMotionTransform
{
	public LoadCompound() : base(IRInstruction.LoadCompound, TransformType.Manual | TransformType.Optimization)
	{
	}
}
