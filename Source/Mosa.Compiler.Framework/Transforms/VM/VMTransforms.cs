// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Transforms.VM;

/// <summary>
/// Transformations
/// </summary>
public static class RuntimeCallTransforms
{
	public static readonly List<BaseTransform> NewList = new List<BaseTransform>
	{
		new NewObject(),
		new NewArray(),
	};
}
