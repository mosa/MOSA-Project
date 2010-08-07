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

namespace Mosa.Runtime.Loader
{
	/// <summary>
	/// Interface to the assembly loader.
	/// </summary>
	public interface IAssemblyLoader
	{
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
		IMetadataModule Load(string file);

		/// <summary>
		/// Loads the named assemblies (as a merged assembly)
		/// </summary>
		/// <param name="file">The file paths of the assemblies to load.</param>
		/// <returns>The assembly image of the loaded assembly.</returns>
		IMetadataModule MergeLoad(IEnumerable<string> files);

		/// <summary>
		/// Unloads the given module.
		/// </summary>
		/// <param name="module">The module to unload.</param>
		void Unload(IMetadataModule module);

		/// <summary>
		/// Gets an enumerable collection of loaded modules.
		/// </summary>
		IEnumerable<IMetadataModule> Modules { get; }

		/// <summary>
		/// Gets the module.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		IMetadataModule GetModule(int index);
	}
}
