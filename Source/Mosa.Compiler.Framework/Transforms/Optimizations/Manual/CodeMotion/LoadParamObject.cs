// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.CodeMotion;

/// <summary>
/// LoadParamObject
/// </summary>
public sealed class LoadParamObject : BaseCodeMotionTransform
{
	public LoadParamObject() : base(IRInstruction.LoadParamObject, TransformType.Manual | TransformType.Optimization)
	{
	}
}
