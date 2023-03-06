// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadObject
/// </summary>
public sealed class LoadObject : BaseCodeMotionTransform
{
	public LoadObject() : base(IRInstruction.LoadObject, TransformType.Manual | TransformType.Optimization)
	{
	}
}
