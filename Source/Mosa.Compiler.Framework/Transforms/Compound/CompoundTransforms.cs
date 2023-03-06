// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Framework.Transforms.Compound;

/// <summary>
/// Transformations
/// </summary>
public static class CompoundTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new LoadCompound(),
		new LoadParamCompound(),
		new MoveCompound(),
		new StoreCompound(),
		new StoreParamCompound(),
	};
}
