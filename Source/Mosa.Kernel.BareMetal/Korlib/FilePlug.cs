// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal.Korlib;

public static class FilePlug
{
	[Plug("System.IO.File::ReadAllBytes")]
	public static byte[] ReadAllBytes(string path) => FileManager.ReadAllBytes(path);

	[Plug("System.IO.File::WriteAllBytes")]
	public static void WriteAllBytes(string path, byte[] bytes) => FileManager.WriteAllBytes(path, bytes);

	[Plug("System.IO.File::WriteAllLines")]
	public static void WriteAllLines(string path, string[] lines) => FileManager.WriteAllLines(path, lines);

	[Plug("System.IO.File::WriteAllText")]
	public static void WriteAllText(string path, string text) => FileManager.WriteAllText(path, text);

	[Plug("System.IO.File::Exists")]
	public static bool Exists(string path) => FileManager.Exists(path);

	[Plug("System.IO.File::Create")]
	public static void Create(string path) => FileManager.Create(path);
}
