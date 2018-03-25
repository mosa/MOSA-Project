// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using System;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime.Korlib.System
{
	internal static class String
	{
		[Method("System.String.InternalAllocateString")]
		internal unsafe static string InternalAllocateString(int length)
		{
			var v = Internal.AllocateString(Intrinsic.GetStringType(), (uint)length);

			var s = Intrinsic.GetObjectFromAddress(v);

			return Unsafe.As<string>(s);
		}
	}
}
