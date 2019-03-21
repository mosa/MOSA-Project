// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Compiler Version
	/// </summary>
	public static class CompilerVersion
	{
		public static int Major = 1;
		public static int Minor = 9;
		public static int Build = 6;

		public static string Version
		{
			get { return $"{Major}.{Minor}.{Build}"; }
		}
	}
}
