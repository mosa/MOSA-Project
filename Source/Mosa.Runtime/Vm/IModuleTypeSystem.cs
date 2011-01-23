/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Metadata.Loader;
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
	public interface IModuleTypeSystem
	{
		/// <summary>
		/// Holds the runtype system
		/// </summary>
		ITypeSystem TypeSystem { get; }

		/// <summary>
		/// Holds the metadata module
		/// </summary>
		IMetadataModule MetadataModule { get; }

		/// <summary>
		/// Gets all types from module.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> GetAllTypes();

		/// <summary>
		/// Retrieves the runtime type for a given metadata token.
		/// </summary>
		/// <param name="token">The token of the type to load. This can represent a typeref, typedef or typespec token.</param>
		/// <returns>The runtime type of the specified token.</returns>
		RuntimeType GetType(TokenTypes token);

		/// <summary>
		/// Retrieves the runtime type for a given type name.
		/// </summary>
		/// <param name="typeName">The name of the type to locate.</param>
		/// <returns>The located <see cref="RuntimeType"/> or null.</returns>
		RuntimeType GetType(string typeName);

		/// <summary>
		/// Gets the runtime type for the given type name and namespace
		/// </summary>
		/// <param name="nameSpace">The name space.</param>
		/// <param name="typeName">Name of the type.</param>
		/// <returns></returns>
		RuntimeType GetType(string nameSpace, string typeName);

		/// <summary>
		/// Retrieves the field definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the field to retrieve.</param>
		/// <returns></returns>
		RuntimeField GetField(TokenTypes token);

		/// <summary>
		/// Retrieves the method definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the method to retrieve.</param>
		/// <returns></returns>
		RuntimeMethod GetMethod(TokenTypes token);

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="callingType">Type of the calling.</param>
		/// <returns></returns>
		RuntimeMethod GetMethod(TokenTypes token, RuntimeType callingType);

		/// <summary>
		/// Adds the internal compiler defined type to the type system
		/// </summary>
		/// <param name="type">The type.</param>
		void AddInternalType(RuntimeType type);
	}
}
