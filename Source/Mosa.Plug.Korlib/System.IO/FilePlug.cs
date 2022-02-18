// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT;
using Mosa.Runtime.Plug;
using System.Collections.Generic;

namespace Mosa.Plug.Korlib.System.IO
{
	public static class FilePlug
	{
		[Plug("System.IO.File::ReadAllBytes")]
		internal static byte[] ReadAllBytes(string path)
		{
			var fat = HAL.GetCurrentFileSystem();
			var entry = fat.FindEntry(path.ToUpper());

			if (!entry.IsValid)
				return null;

			var stream = new FatFileStream(fat, entry);
			var bytes = new byte[stream.Length];

			stream.Read(bytes, 0, bytes.Length);

			return bytes;
		}

		[Plug("System.IO.File::WriteAllBytes")]
		internal static void WriteAllBytes(string path, byte[] bytes)
		{
			var fat = HAL.GetCurrentFileSystem();

			if (fat.IsReadOnly)
				return;

			var upper = path.ToUpper();
			var existing = fat.FindEntry(upper);

			var entry = existing.IsValid ? existing : fat.CreateFile(upper, FatFileAttributes.Unused);
			var stream = new FatFileStream(fat, entry);

			stream.Write(bytes, 0, bytes.Length);
		}

		[Plug("System.IO.File::WriteAllText")]
		internal static void WriteAllText(string path, string text)
		{
			var fat = HAL.GetCurrentFileSystem();

			if (fat.IsReadOnly)
				return;

			var upper = path.ToUpper();
			var existing = fat.FindEntry(upper);

			var entry = existing.IsValid ? existing : fat.CreateFile(upper, FatFileAttributes.Unused);
			var stream = new FatFileStream(fat, entry);

			var list = new List<byte>();
			foreach (var c in text)
				list.Add((byte)c);

			var bytes = list.ToArray();

			stream.Write(bytes, 0, bytes.Length);
		}

		[Plug("System.IO.File::WriteAllLines")]
		internal static void WriteAllLines(string path, string[] lines)
		{
			var fat = HAL.GetCurrentFileSystem();

			if (fat.IsReadOnly)
				return;

			var upper = path.ToUpper();
			var existing = fat.FindEntry(upper);

			var entry = existing.IsValid ? existing : fat.CreateFile(upper, FatFileAttributes.Unused);
			var stream = new FatFileStream(fat, entry);

			var list = new List<byte>();
			foreach (var str in lines)
			{
				foreach (var c in str)
					list.Add((byte)c);
				list.Add((byte)'\n');
			}

			var bytes = list.ToArray();

			stream.Write(bytes, 0, bytes.Length);
		}

		[Plug("System.IO.File::Create")]
		internal static void Create(string path)
		{
			var fat = HAL.GetCurrentFileSystem();

			if (fat.IsReadOnly)
				return;

			var upper = path.ToUpper();

			if (fat.FindEntry(upper).IsValid)
				return;

			_ = fat.CreateFile(upper, FatFileAttributes.Unused);
		}

		[Plug("System.IO.File::Exists")]
		internal static bool Exists(string path)
		{
			return HAL.GetCurrentFileSystem().FindEntry(path.ToUpper()).IsValid;
		}
	}
}
