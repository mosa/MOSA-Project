// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transforms.TruncateTo32;

/// <summary>
/// TruncateTo32 Transforms
/// </summary>
public static class TruncateTo32Transforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new Store32(),
	};
}
