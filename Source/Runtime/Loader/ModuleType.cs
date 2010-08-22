/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.Loader
{
	/// <summary>
	/// Specifies the type of the module.
	/// </summary>
	public enum ModuleType
	{
		/// <summary>
		/// The module is an executable file.
		/// </summary>
		Executable,

		/// <summary>
		/// The module is a library.
		/// </summary>
		Library
	}
}
