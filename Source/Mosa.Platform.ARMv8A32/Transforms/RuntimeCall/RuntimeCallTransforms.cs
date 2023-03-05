// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.RuntimeCall;

/// <summary>
/// RuntimeCall Transformation List
/// </summary>
public static class RuntimeCallTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
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
	};
}
