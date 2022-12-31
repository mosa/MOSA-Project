// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Tweak
{
	/// <summary>
	/// Tweak Transformation List
	/// </summary>
	public static class TweakTransforms
	{
		public static readonly List<BaseTransform> List = new List<BaseTransform>
		{
			new Blsr32(),
			new Blsr64(),
			new Cmp32(),
			new Cmp64(),
			new CMov32(),
			new CMov64(),
			new Mov32(),
			new Mov64(),
			new MovLoad16(),
			new MovLoad8(),
			new Movsd(),
			new Movss(),
			new MovStore16(),
			new MovStore8(),
			new Movzx16To32(),
			new Movzx16To64(),
			new Movzx8To32(),
			new Movzx8To64(),
			new Nop(),
			new Setcc(),
			//new Popcnt32(),
			//new Tzcnt32(),
			//new Lzcnt32(),
			//new Popcnt64(),
			//new Tzcnt64(),
			//new Lzcnt64(),
			};
	}
}
