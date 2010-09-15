/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Loader
{
	/// <summary>
	/// Interface to the assembly loader.
	/// </summary>
	public interface IAssemblyLoader
	{

		/// <summary>
		/// Initializes the private paths.
		/// </summary>
		/// <param name="assemblyPaths">The assembly paths.</param>
		void InitializePrivatePaths(IEnumerable<string> assemblyPaths);

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		void AppendPrivatePath(string path);

		/// <summary>
		/// Loads the named assembly.
		/// </summary>
		/// <param name="file">The file path of the assembly to load.</param>
		/// <returns>The assembly image of the loaded assembly.</returns>
		IMetadataModule LoadModule(string file);

	}
}
