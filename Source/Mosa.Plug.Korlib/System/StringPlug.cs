// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Plug;
using System.Runtime.CompilerServices;

namespace Mosa.Plug.Korlib.System
{
	internal static class StringPlug
	{
		[Plug("System.String::InternalAllocateString")]
		internal static string InternalAllocateString(int length)
		{
			var v = Mosa.Runtime.Internal.AllocateString(Intrinsic.GetStringType(), (uint)length);

			var s = Intrinsic.GetObjectFromAddress(v);

			return Unsafe.As<string>(s);
		}
	}
}
