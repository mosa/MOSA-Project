// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Expand;

/// <summary>
/// Transformations
/// </summary>
public static class IRTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		new CheckArrayBounds(),

		new ThrowIndexOutOfRange(),
		new ThrowOverflow(),
		new ThrowDivideByZero(),

		new CheckThrowIndexOutOfRange(),
		new CheckThrowOverflow(),
		new CheckThrowDivideByZero(),

		new Switch(),
	};
}
