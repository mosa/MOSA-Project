// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime;
using Mosa.Runtime.Plug;

namespace Mosa.Plug.Korlib.System;

internal static class Marshal
{
	[Plug("System.Runtime.InteropServices.Marshal::GetFunctionPointerForDelegateInternal")]
	internal static IntPtr GetFunctionPointerForDelegateInternal(Delegate d)
	{
		return Intrinsic.GetObjectAddress(d).LoadPointer(0).ToIntPtr();
	}
}
