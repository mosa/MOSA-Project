// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.Globalization
{
	public static class CharUnicodeInfo
	{
		internal const char HIGH_SURROGATE_START = '\uD800';
		internal const char HIGH_SURROGATE_END = '\uDBFF';
		internal const char LOW_SURROGATE_START = '\uDC00';
		internal const char LOW_SURROGATE_END = '\uDFFF';
		internal const int UNICODE_CATEGORY_OFFSET = 0;
		internal const int BIDI_CATEGORY_OFFSET = 1;

		[StructLayout(LayoutKind.Explicit)]
		internal struct UnicodeDataHeader
		{
			[FieldOffset(0)]
			internal char TableName;

			[FieldOffset(32)]
			internal ushort version;

			[FieldOffset(40)]
			internal uint OffsetToCategoriesIndex;

			[FieldOffset(44)]
			internal uint OffsetToCategoriesValue;

			[FieldOffset(48)]
			internal uint OffsetToNumbericIndex;

			[FieldOffset(52)]
			internal uint OffsetToDigitValue;

			[FieldOffset(56)]
			internal uint OffsetToNumbericValue;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		internal struct DigitValues
		{
			internal sbyte decimalDigit;
			internal sbyte digit;
		}
	}
}
