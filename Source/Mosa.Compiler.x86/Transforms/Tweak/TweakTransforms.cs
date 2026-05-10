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
		Cmp32.Instance,
		Test32.Instance,
		Shl32.Instance,
		Shld32.Instance,
		Shr32.Instance,
		Shrd32.Instance,
		CMov32.Instance,
		Blsr32.Instance,
		Popcnt32.Instance,
		Tzcnt32.Instance,
		Lzcnt32.Instance,

		Mov32.Instance,
		MovLoad16.Instance,
		MovLoad8.Instance,
		Movsd.Instance,
		Movss.Instance,
		MovStore16.Instance,
		MovStore8.Instance,
		Movsx16To32.Instance,
		Movsx8To32.Instance,
		Movzx16To32.Instance,
		Movzx8To32.Instance,
		Setcc.Instance,
	};
}
