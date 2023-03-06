// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadR4
/// </summary>
public sealed class LoadR4 : BaseCodeMotionTransform
{
	public LoadR4() : base(IRInstruction.LoadR4, TransformType.Manual | TransformType.Optimization)
	{
	}
}
