// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Plug;

/// <summary>
/// Transformations
/// </summary>
public static class PlugTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		CallDirect.Instance,
		CallStatic.Instance,
		CallVirtual.Instance,
	};
}
