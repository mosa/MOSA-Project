// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System
{
	public static class Console
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResetColor();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Clear();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WriteLine();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WriteLine(string value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Write(string value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetCursorPosition(int left, int top);
	}
}