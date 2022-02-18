// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System.IO
{
	public class File
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte[] ReadAllBytes(string path);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WriteAllBytes(string path, byte[] bytes);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WriteAllText(string path, string text);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void WriteAllLines(string path, string[] lines);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Create(string path);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Exists(string path);
	}
}
