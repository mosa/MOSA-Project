// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System
{
	public static class Console
	{
		public static ConsoleColor ForegroundColor
		{
			get
			{
				return GetForegroundColor();
			}
			set
			{
				SetForegroundColor(value);
			}
		}

		public static ConsoleColor BackgroundColor
		{
			get
			{
				return GetBackgroundColor();
			}
			set
			{
				SetBackgroundColor(value);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ConsoleColor GetForegroundColor();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ConsoleColor GetBackgroundColor();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetForegroundColor(ConsoleColor color);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetBackgroundColor(ConsoleColor color);

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