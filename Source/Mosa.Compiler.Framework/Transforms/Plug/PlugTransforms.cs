// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transforms.Plug;

/// <summary>
/// Transformations
/// </summary>
public static class PlugTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new CallDirect(),
		new CallStatic(),
		new CallVirtual(),
	};
}
