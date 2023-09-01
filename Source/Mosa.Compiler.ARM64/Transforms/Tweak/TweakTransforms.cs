// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM64.Transforms.Tweak;

/// <summary>
/// Tweak Transformation List
/// </summary>
public static class TweakTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		//new Mov(),
		//new Mvf(),
	};
}
