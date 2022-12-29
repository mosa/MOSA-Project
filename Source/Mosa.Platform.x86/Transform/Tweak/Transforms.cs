// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.Tweak
{
	/// <summary>
	/// Transformations
	/// </summary>
	public static class Transforms
	{
		public static readonly List<BaseTransformation> TweakList = new List<BaseTransformation>
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
