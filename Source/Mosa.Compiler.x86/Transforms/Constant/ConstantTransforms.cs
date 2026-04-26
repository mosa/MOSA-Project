// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Constant;

/// <summary>
/// Tweak Transformation List
/// </summary>
public static class ConstantTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		new Movsx16To32(),
		new Movsx8To32(),
	};
}
