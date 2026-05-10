// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// Checked Conversion Transforms
/// </summary>
public static class CheckedConversionTransforms
{
	public static readonly List<BaseTransform> List = new()
	{
		CheckedConversionI32ToI16.Instance,
		CheckedConversionI32ToU16.Instance,
		CheckedConversionI32ToU32.Instance,
		CheckedConversionI32ToU64.Instance,
		CheckedConversionI64ToI16.Instance,
		CheckedConversionI64ToI8.Instance,
		CheckedConversionI64ToU16.Instance,
		CheckedConversionI64ToU32.Instance,
		CheckedConversionI64ToU64.Instance,
		CheckedConversionR4ToI16.Instance,
		CheckedConversionR4ToI64.Instance,
		CheckedConversionR4ToI8.Instance,
		CheckedConversionR4ToU16.Instance,
		CheckedConversionR4ToU32.Instance,
		CheckedConversionR4ToU64.Instance,
		CheckedConversionR8ToI16.Instance,
		CheckedConversionR8ToI64.Instance,
		CheckedConversionR8ToI8.Instance,
		CheckedConversionR8ToU16.Instance,
		CheckedConversionR8ToU32.Instance,
		CheckedConversionR8ToU64.Instance,
		CheckedConversionU32ToI16.Instance,
		CheckedConversionU32ToI8.Instance,
		CheckedConversionU32ToU16.Instance,
		CheckedConversionU64ToI16.Instance,
		CheckedConversionU64ToI64.Instance,
		CheckedConversionU64ToI8.Instance,
		CheckedConversionU64ToU16.Instance,
		CheckedConversionU64ToU32.Instance,
		CheckedConversionU64ToI32.Instance,
		CheckedConversionI64ToU8.Instance,
		CheckedConversionR4ToI32.Instance,
		CheckedConversionR8ToI32.Instance,
		CheckedConversionI64ToI32.Instance
	};
}
