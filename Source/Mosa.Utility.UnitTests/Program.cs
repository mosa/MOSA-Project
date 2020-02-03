// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System;

namespace Mosa.Utility.UnitTests
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				RegisterPlatforms();

				if (UnitTestSystem.Start(args) == 0)
				{
					Environment.Exit(0);
				}
				else
				{
					Environment.Exit(1);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				Environment.Exit(1);
			}
		}

		private static void RegisterPlatforms()
		{
			PlatformRegistry.Add(new Platform.x86.Architecture());
			PlatformRegistry.Add(new Platform.x64.Architecture());

			//PlatformRegistry.Add(new Platform.ARMv8A32.Architecture());
		}
	}
}
