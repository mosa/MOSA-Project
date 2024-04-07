// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Tweak;

/// <summary>
/// Tweak Transformation List
/// </summary>
public static class TweakTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		new Cmp32(),
		new Test32(),
		new Shl32(),
		new Shld32(),
		new Shr32(),
		new Shrd32(),
		new CMov32(),
		new Blsr32(),
		new Popcnt32(),
		new Tzcnt32(),
		new Lzcnt32(),

		new Mov32(),
		new MovLoad16(),
		new MovLoad8(),
		new Movsd(),
		new Movss(),
		new MovStore16(),
		new MovStore8(),
		new Movsx16To32(),
		new Movsx8To32(),
		new Movzx16To32(),
		new Movzx8To32(),
		new Setcc(),
	};
}
