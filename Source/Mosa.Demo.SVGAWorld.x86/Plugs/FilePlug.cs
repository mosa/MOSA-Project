// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using Mosa.FileSystem.FAT;
using Mosa.Runtime.Plug;

namespace Mosa.Demo.SVGAWorld.x86.Plugs;

public static class FilePlug
{
	private static List<FatFileSystem> fileSystems;
	private static int currentDrive = 0;

	public static void Register(FatFileSystem fat)
	{
		fileSystems ??= new List<FatFileSystem>();

		if (fat.IsValid)
			fileSystems.Add(fat);
	}

	[Plug("System.IO.File::ReadAllBytes")]
	internal static byte[] ReadAllBytes(string path)
	{
		var fat = fileSystems[currentDrive];
		var entry = fat.FindEntry(path.ToUpper());

		if (!entry.IsValid)
			return null;

		var stream = new FatFileStream(fat, entry);
		var bytes = new byte[stream.Length];

		stream.Read(bytes, 0, bytes.Length);

		return bytes;
	}

	[Plug("System.IO.File::ReadAllLines")]
	internal static string[] ReadAllLines(string path)
	{
		throw new NotImplementedException();
	}

	[Plug("System.IO.File::ReadAllText")]
	internal static string ReadAllText(string path)
	{
		throw new NotImplementedException();
	}

	[Plug("System.IO.File::WriteAllBytes")]
	internal static void WriteAllBytes(string path, byte[] bytes)
	{
		var fat = fileSystems[currentDrive];

		if (fat.IsReadOnly)
			return;

		var upper = path.ToUpper();
		var existing = fat.FindEntry(upper);

		var entry = existing.IsValid ? existing : fat.CreateFile(upper, FatFileAttributes.Unused);
		var stream = new FatFileStream(fat, entry);

		stream.Write(bytes, 0, bytes.Length);
	}

	[Plug("System.IO.File::WriteAllLines")]
	internal static void WriteAllLines(string path, string[] lines)
	{
		var fat = fileSystems[currentDrive];

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

	[Plug("System.IO.File::WriteAllText")]
	internal static void WriteAllText(string path, string text)
	{
		var fat = fileSystems[currentDrive];

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

	[Plug("System.IO.File::Exists")]
	internal static bool Exists(string path) => fileSystems[currentDrive].FindEntry(path.ToUpper()).IsValid;

	[Plug("System.IO.File::Create")]
	internal static void Create(string path)
	{
		var fat = fileSystems[currentDrive];

		if (fat.IsReadOnly)
			return;

		var upper = path.ToUpper();

		if (fat.FindEntry(upper).IsValid)
			return;

		fat.CreateFile(upper, FatFileAttributes.Unused);
	}
}
