// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.BasicBlocks;

/// <summary>
/// Basic Blocks Transformations
/// </summary>
public static class BasicBlocksTransforms
{
	public static readonly List<BaseBlockTransform> List = new List<BaseBlockTransform>
	{
		new MergeBlocks(),
		new RemoveUnreachableBlocks(),
		new SkipEmptyBlocks(),
	};
}
