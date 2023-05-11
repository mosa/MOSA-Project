// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transforms.CheckedConversion;

/// <summary>
/// Checked Conversion Transforms
/// </summary>
public static class CheckedConversionTransforms
{
	public static readonly List<BaseTransform> List = new List<BaseTransform>
	{
		new CheckedConversionI32ToI16(),
		new CheckedConversionI32ToU16(),
		new CheckedConversionI32ToU32(),
		new CheckedConversionI32ToU64(),
		new CheckedConversionI64ToI16(),
		new CheckedConversionI64ToI8(),
		new CheckedConversionI64ToU16(),
		new CheckedConversionI64ToU32(),
		new CheckedConversionI64ToU64(),
		new CheckedConversionR4ToI16(),
		new CheckedConversionR4ToI64(),
		new CheckedConversionR4ToI8(),
		new CheckedConversionR4ToU16(),
		new CheckedConversionR4ToU32(),
		new CheckedConversionR4ToU64(),
		new CheckedConversionR8ToI16(),
		new CheckedConversionR8ToI64(),
		new CheckedConversionR8ToI8(),
		new CheckedConversionR8ToU16(),
		new CheckedConversionR8ToU32(),
		new CheckedConversionR8ToU64(),
		new CheckedConversionU32ToI16(),
		new CheckedConversionU32ToI8(),
		new CheckedConversionU32ToU16(),
		new CheckedConversionU64ToI16(),
		new CheckedConversionU64ToI64(),
		new CheckedConversionU64ToI8(),
		new CheckedConversionU64ToU16(),
		new CheckedConversionU64ToU32(),
		new CheckedConversionU64ToI32(),
		new CheckedConversionI64ToU8(),
		new CheckedConversionR4ToI32(),
		new CheckedConversionR8ToI32(),
		new CheckedConversionI64ToI32()
	};
}
