// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM64.Transforms.Stack;

/// <summary>
/// Stack Build Transformation List
/// </summary>
public static class StackTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		//new Epilogue(),
		//new Prologue(),
	};
}
