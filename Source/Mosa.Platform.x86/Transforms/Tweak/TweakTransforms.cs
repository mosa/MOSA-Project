// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms;
using System.Collections.Generic;

namespace Mosa.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Tweak Transformation List
	/// </summary>
	public static class StackBuildTransforms
	{
		public static readonly List<BaseTransformation> List = new List<BaseTransformation>
		{
			//new Call(),
			new Cmp32(),
			new Shl32(),
			new Shld32(),
			new Shr32(),
			new Shrd32(),
			new CMov32(),
			new Blsr32(),
			new Popcnt32(),
			new Tzcnt32(),
			new Lzcnt32(),
		};
	}
}
