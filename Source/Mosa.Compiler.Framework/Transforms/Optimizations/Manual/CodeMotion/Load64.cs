// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// Load64
/// </summary>
public sealed class Load64 : BaseCodeMotionTransform
{
	public Load64() : base(IRInstruction.Load64, TransformType.Manual | TransformType.Optimization)
	{
	}
}
