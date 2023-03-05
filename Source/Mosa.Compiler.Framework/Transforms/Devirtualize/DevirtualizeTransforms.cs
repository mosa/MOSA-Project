// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;
using Mosa.Compiler.Framework.Transforms.Devirtualize;

namespace Mosa.Platform.Framework.Devirtualize;

/// <summary>
/// Transformations
/// </summary>
public static class DevirtualizeTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new CallVirtual(),
	};
}
