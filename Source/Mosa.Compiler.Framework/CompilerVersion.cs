// Copyright (c) MOSA Project. Licensed under the New BSD License.

/// <summary>
///
/// </summary>
namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Version
	/// </summary>
	public static class CompilerVersion
	{
		public static int Major = 1;
		public static int Minor = 9;
		public static int Build = 0;

		public static string Version
		{
			get { return $"{Major}.{Minor}.{Build}"; }
		}
	}
}
