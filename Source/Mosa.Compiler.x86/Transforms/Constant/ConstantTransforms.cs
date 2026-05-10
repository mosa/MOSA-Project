// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Constant;

/// <summary>
/// Constant Transformation List
/// </summary>
public static class ConstantTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		Movsx16To32.Instance,
		Movsx8To32.Instance,
		Cmp32.Instance,
	};
}
