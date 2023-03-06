// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

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
