// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.RuntimeCall;

/// <summary>
/// RuntimeCall Transformation List
/// </summary>
public static class RuntimeCallTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		DivSigned32.Instance,
		DivSigned64.Instance,
		DivUnsigned64.Instance,
		DivUnsigned32.Instance,
		RemSigned32.Instance,
		RemSigned64.Instance,
		RemUnsigned64.Instance,
		RemUnsigned32.Instance,
		ConvertR4ToR8.Instance,
		ConvertR8ToR4.Instance,
		BitCopyR4To32.Instance,
		BitCopy32ToR4.Instance,
		MulCarryOut64.Instance,
		MulOverflowOut64.Instance,
	};
}
