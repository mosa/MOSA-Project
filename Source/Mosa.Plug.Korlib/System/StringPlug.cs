// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using Mosa.Runtime;
using Mosa.Runtime.Plug;

namespace Mosa.Plug.Korlib.System;

internal static class StringPlug
{
	[Plug("System.String::InternalAllocateString")]
	internal static string InternalAllocateString(int length)
	{
		// Future: Lookup TypeDef/MethodPointer on String.Empty (for example)

		var v = Mosa.Runtime.Internal.AllocateString(Intrinsic.GetStringType(), (uint)length);

		var s = Intrinsic.GetObjectFromAddress(v);

		return Unsafe.As<string>(s);
	}
}
