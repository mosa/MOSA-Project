// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.Plug;

namespace Mosa.Demo.TestWorld.x64.Plugs;

public static class FilePlug
{
	[Plug("System.IO.File::ReadAllBytes")]
	internal static byte[] ReadAllBytes(string path)
	{
		throw new NotImplementedException();
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
		throw new NotImplementedException();
	}

	[Plug("System.IO.File::WriteAllLines")]
	internal static void WriteAllLines(string path, string[] lines)
	{
		throw new NotImplementedException();
	}

	[Plug("System.IO.File::WriteAllText")]
	internal static void WriteAllText(string path, string text)
	{
		throw new NotImplementedException();
	}

	[Plug("System.IO.File::Exists")]
	internal static bool Exists(string path)
	{
		throw new NotImplementedException();
	}

	[Plug("System.IO.File::Create")]
	internal static void Create(string path)
	{
		throw new NotImplementedException();
	}
}
