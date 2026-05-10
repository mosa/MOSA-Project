// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.RuntimeCall;

/// <summary>
/// RuntimeCall Transformation List
/// </summary>
public static class RuntimeCallTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		DivSigned64.Instance,
		DivUnsigned64.Instance,
		RemR4.Instance,
		RemR8.Instance,
		RemSigned64.Instance,
		RemUnsigned64.Instance,
		ConvertR4ToI64.Instance,
		ConvertR4ToU32.Instance,
		ConvertR8ToI32.Instance,
		ConvertR8ToI64.Instance,
		ConvertR4ToU64.Instance,
		ConvertR8ToU32.Instance,
		ConvertR8ToU64.Instance,
		MulCarryOut64.Instance,
		MulOverflowOut64.Instance,
	};
}
