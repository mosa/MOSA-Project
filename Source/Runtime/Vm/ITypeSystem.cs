/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.Vm
{
	/// <summary>
	/// The interface of the type system.
	/// </summary>
	/// <remarks>
	/// The type system is responsible for loading assembly metadata and building
	/// runtime accessible management structures from those.
	/// </remarks>
	public interface ITypeSystem
	{

		/// <summary>
		/// Loads the modules.
		/// </summary>
		/// <param name="files">The files.</param>
		void LoadModules(IEnumerable<string> files);

		/// <summary>
		/// Adds the module reference.
		/// </summary>
		/// <param name="file">The file.</param>
		IModuleTypeSystem ResolveModuleReference(string file);

		/// <summary>
		/// Gets the main module type system.
		/// </summary>
		/// <returns></returns>
		IModuleTypeSystem GetMainModuleTypeSystem();

		/// <summary>
		/// Retrieves the runtime type for a given type name.
		/// </summary>
		/// <param name="typeName">The name of the type to locate.</param>
		/// <returns>The located <see cref="RuntimeType"/> or null.</returns>
		RuntimeType GetType(string typeName);

		/// <summary>
		/// Gets the compiled types.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> GetCompiledTypes();

		/// <summary>
		/// Gets the internal module type system.
		/// </summary>
		/// <value>The internal module type system.</value>
		IModuleTypeSystem InternalModuleTypeSystem { get; }

		/// <summary>
		/// Adds the internal compiler defined type to the type system
		/// </summary>
		/// <param name="type">The type.</param>
		void AddInternalType(RuntimeType type);

	}
}
