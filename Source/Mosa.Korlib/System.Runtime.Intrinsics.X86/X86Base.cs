// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;

namespace System.Runtime.Intrinsics.X86
{
	public abstract partial class X86Base
	{
		internal X86Base()
		{ }

		public static bool IsSupported
		{ get { throw null; } }

		//public static (int Eax, int Ebx, int Ecx, int Edx) CpuId(int functionId, int subFunctionId)
		//{ throw null; }

		public static void Pause()
		{ throw null; }

		public abstract partial class X64
		{
			internal X64()
			{ }

			public static bool IsSupported
			{ get { throw null; } }
		}
	}
}
