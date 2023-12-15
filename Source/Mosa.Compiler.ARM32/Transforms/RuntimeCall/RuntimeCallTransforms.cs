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
		new DivSigned32(),
		new DivSigned64(),
		new DivUnsigned64(),
		new DivUnsigned32(),
		new RemSigned32(),
		new RemSigned64(),
		new RemUnsigned64(),
		new RemUnsigned32(),
		new ConvertR4ToR8(),
		new ConvertR8ToR4(),
		new BitCopyR4To32(),
		new BitCopy32ToR4(),
		new MulCarryOut64(),
		new MulOverflowOut64(),
	};
}
