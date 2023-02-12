// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transforms.IR;

/// <summary>
/// Transformations
/// </summary>
public static class IRTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new CheckArrayBounds(),

		new ThrowIndexOutOfRange(),
		new ThrowOverflow(),
		new ThrowDivideByZero(),
	};
}
