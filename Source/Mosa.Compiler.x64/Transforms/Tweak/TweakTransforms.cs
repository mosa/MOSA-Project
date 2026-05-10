// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Tweak;

/// <summary>
/// Tweak Transformation List
/// </summary>
public static class TweakTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		Blsr32.Instance,
		Blsr64.Instance,
		Cmp32.Instance,
		Cmp64.Instance,
		CMov32.Instance,
		CMov64.Instance,
		Mov32.Instance,
		Mov64.Instance,
		MovLoad16.Instance,
		MovLoad8.Instance,
		Movsd.Instance,
		Movss.Instance,
		MovStore16.Instance,
		MovStore8.Instance,
		Movzx16To32.Instance,
		Movzx16To64.Instance,
		Movzx8To32.Instance,
		Movzx8To64.Instance,
		Nop.Instance,
		Setcc.Instance,
		//Popcnt32.Instance,
		//Tzcnt32.Instance,
		//Lzcnt32.Instance,
		//Popcnt64.Instance,
		//Tzcnt64.Instance,
		//Lzcnt64.Instance,
	};
}
