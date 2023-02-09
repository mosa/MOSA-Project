// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Demo.SVGAWorld.x86.Plugs;

public static class FilePlug
{
	[Plug("System.IO.File::ReadAllBytes")]
	internal static byte[] ReadAllBytes(string path) => FileManager.ReadAllBytes(path);

	[Plug("System.IO.File::WriteAllBytes")]
	internal static void WriteAllBytes(string path, byte[] bytes) => FileManager.WriteAllBytes(path, bytes);

	[Plug("System.IO.File::WriteAllLines")]
	internal static void WriteAllLines(string path, string[] lines) => FileManager.WriteAllLines(path, lines);

	[Plug("System.IO.File::WriteAllText")]
	internal static void WriteAllText(string path, string text) => FileManager.WriteAllText(path, text);

	[Plug("System.IO.File::Exists")]
	internal static bool Exists(string path) => FileManager.Exists(path);

	[Plug("System.IO.File::Create")]
	internal static void Create(string path) => FileManager.Create(path);
}
