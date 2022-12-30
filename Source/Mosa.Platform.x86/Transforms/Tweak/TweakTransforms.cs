// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Tweak Transformation List
	/// </summary>
	public static class TweakTransforms
	{
		public static readonly List<BaseTransformation> List = new List<BaseTransformation>
		{
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
