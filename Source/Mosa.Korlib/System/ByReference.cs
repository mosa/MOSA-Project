// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System;

internal readonly ref struct ByReference<T>
{
	private readonly IntPtr _value;

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
