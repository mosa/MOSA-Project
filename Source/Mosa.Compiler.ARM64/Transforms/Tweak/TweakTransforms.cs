// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM64.Transforms.Tweak;

/// <summary>
/// Tweak Transformation List
/// </summary>
public static class TweakTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		//new Mov(),
		//new Mvf(),
	};
}
