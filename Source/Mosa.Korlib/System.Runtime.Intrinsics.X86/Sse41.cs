// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Licensed to the.NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace System.Runtime.Intrinsics.X86;

/// <summary>
/// This class provides access to Intel SSE4.1 hardware instructions via intrinsics
/// </summary>
[Intrinsic]
[CLSCompliant(false)]
public abstract class Sse41 : Ssse3
{
	internal Sse41()
	{ }

	public new static bool IsSupported { get => IsSupported; }

	[Intrinsic]
	public abstract new class X64 : Ssse3.X64
	{
		internal X64()
		{ }

		public new static bool IsSupported { get => IsSupported; }
	}
}
