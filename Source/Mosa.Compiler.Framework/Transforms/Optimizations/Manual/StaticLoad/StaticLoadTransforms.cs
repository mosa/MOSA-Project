// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StaticLoad;

/// <summary>
/// Transformations
/// </summary>
public static class StaticLoadTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		new Load32(),
		new Load64(),
	};
}
