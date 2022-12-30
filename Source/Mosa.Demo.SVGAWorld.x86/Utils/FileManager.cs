// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.FileSystem.FAT;

namespace Mosa.Demo.SVGAWorld.x86
{
	public static class FileManager
	{
		private static List<FatFileSystem> FAT;

		public static int CurrentDrive = 0;

		public static void Register(FatFileSystem fat)
		{
			if (FAT == null)
				FAT = new List<FatFileSystem>();

			if (fat.IsValid)
				FAT.Add(fat);
		}

		public static byte[] ReadAllBytes(string path)
		{
			var fat = FAT[CurrentDrive];

			var entry = fat.FindEntry(path.ToUpper());

			if (!entry.IsValid)
				return null;

			var stream = new FatFileStream(fat, entry);
			var bytes = new byte[stream.Length];

			stream.Read(bytes, 0, bytes.Length);

			return bytes;
		}

		public static void WriteAllBytes(string path, byte[] bytes)
		{
			var fat = FAT[CurrentDrive];

			if (fat.IsReadOnly)
				return;

			var upper = path.ToUpper();
			var existing = fat.FindEntry(upper);

			var entry = existing.IsValid ? existing : fat.CreateFile(upper, FatFileAttributes.Unused);
			var stream = new FatFileStream(fat, entry);

			stream.Write(bytes, 0, bytes.Length);
		}

		public static void WriteAllText(string path, string text)
		{
			var fat = FAT[CurrentDrive];

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

		public static void WriteAllLines(string path, string[] lines)
		{
			var fat = FAT[CurrentDrive];

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

		public static void CreateFile(string path)
		{
			var fat = FAT[CurrentDrive];

			if (fat.IsReadOnly)
				return;

			var upper = path.ToUpper();

			if (fat.FindEntry(upper).IsValid)
				return;

			_ = fat.CreateFile(upper, FatFileAttributes.Unused);
		}

		public static bool Exists(string path)
		{
			return FAT[CurrentDrive].FindEntry(path.ToUpper()).IsValid;
		}
	}
}
