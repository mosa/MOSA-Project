// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Utility.UnitTests
{
	internal static class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				if (UnitTestSystem.Start(args.Length > 0) == 0)
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
	}
}
