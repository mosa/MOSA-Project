// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transforms.PlatformIntrinsic;

/// <summary>
/// Transformations
/// </summary>
public static class PlatformIntrinsicTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new IntrinsicMethodCall(),
	};
}
