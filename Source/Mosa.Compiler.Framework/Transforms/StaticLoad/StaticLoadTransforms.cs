// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.Framework.Transforms.StaticLoad;

/// <summary>
/// Transformations
/// </summary>
public static class StaticLoadTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new Load32(),
		new Load64(),
	};
}
