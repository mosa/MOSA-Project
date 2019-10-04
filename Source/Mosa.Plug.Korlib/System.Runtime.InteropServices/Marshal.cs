// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Plug;
using System;

namespace Mosa.Plug.Korlib.System
{
	internal static class Marshal
	{
		[Plug("System.Runtime.InteropServices.Marshal::GetFunctionPointerForDelegateInternal")]
		internal static IntPtr GetFunctionPointerForDelegateInternal(Delegate d)
		{
			return Intrinsic.GetObjectAddress(d).LoadPointer(Pointer.Size * 2).ToIntPtr();
		}
	}
}
