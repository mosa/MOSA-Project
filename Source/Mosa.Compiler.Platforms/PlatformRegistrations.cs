// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platforms;

public static class PlatformRegistrations
{
	public static void Register()
	{
		PlatformRegistry.Register();
		PlatformRegistry.Add(new Mosa.Compiler.x86.Architecture());
		PlatformRegistry.Add(new Mosa.Compiler.x64.Architecture());
		PlatformRegistry.Add(new Mosa.Compiler.ARM32.Architecture());
		//PlatformRegistry.Add(new Mosa.Compiler.ARM64.Architecture());
	}
}
