// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transforms.Array;

/// <summary>
/// Transformations
/// </summary>
public static class ArrayTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new CheckArrayBounds(),
	};
}
