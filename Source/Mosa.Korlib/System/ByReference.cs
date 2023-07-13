// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System;

internal readonly struct ByReference<T>
{
#pragma warning disable CS0169 // The field is never used
	private readonly IntPtr _value;
#pragma warning restore CS0169 // The field is never used

	[Intrinsic]
	public ByReference(ref T value)
	{
		throw new PlatformNotSupportedException();
	}

	public ref T Value
	{
		[Intrinsic]
		get => throw new PlatformNotSupportedException();
	}
}
