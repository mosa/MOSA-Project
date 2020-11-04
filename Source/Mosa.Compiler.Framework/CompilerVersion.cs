// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Reflection;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Compiler Version
	/// </summary>
	public static class CompilerVersion
	{
		private static readonly Version Version = GetVersion();

		public static Version GetVersion()
		{
			var version = Assembly.GetExecutingAssembly().GetName().Version;

			if (version.Build == 0)
			{
				// Revision and build number are reversed by design
				version = new Version(2, 0, 0, 2);
			}

			return version;
		}

		public static string VersionString
		{
			get { return $"{Version.Major}.{Version.Minor}.{Version.Revision}.{Version.Build}"; }
		}
	}
}
