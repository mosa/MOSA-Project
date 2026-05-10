// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// Transformations
/// </summary>
public static class CompoundTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		LoadCompound.Instance,
		LoadParamCompound.Instance,
		MoveCompound.Instance,
		StoreCompound.Instance,
		StoreParamCompound.Instance,
	};
}
