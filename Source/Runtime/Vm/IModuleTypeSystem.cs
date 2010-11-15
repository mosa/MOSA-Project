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
	public interface IModuleTypeSystem
	{
		/// <summary>
		/// Holds the runtype system
		/// </summary>
		ITypeSystem TypeSystem { get; }

		/// <summary>
		/// Holds the metadata module
		/// </summary>
		IMetadataModule MetadataModule { get;  }

		/// <summary>
		/// Array of loaded runtime type descriptors.
		/// </summary>
		RuntimeType[] Types { get; }

		/// <summary>
		/// Holds all loaded method definitions.
		/// </summary>
		RuntimeMethod[] Methods { get; }

		/// <summary>
		/// Holds all parameter information elements.
		/// </summary>
		RuntimeParameter[] Parameters { get; }

		/// <summary>
		/// Holds all loaded _stackFrameIndex definitions.
		/// </summary>
		RuntimeField[] Fields { get; }

		/// <summary>
		/// Gets the types from module.
		/// </summary>
		/// <returns></returns>
		ReadOnlyRuntimeTypeListView GetTypes();

		/// <summary>
		/// Gets all types from module.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> GetAllTypes();

		/// <summary>
		/// Retrieves the runtime type for a given metadata token.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="token">The token of the type to load. This can represent a typeref, typedef or typespec token.</param>
		/// <returns>The runtime type of the specified token.</returns>
		RuntimeType GetType(ISignatureContext context, TokenTypes token);

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
		/// Retrieves the stackFrameIndex definition identified by the given token in the scope.
		/// </summary>
		/// <param name="context">The generic parameter resolution context.</param>
		/// <param name="token">The token of the _stackFrameIndex to retrieve.</param>
		/// <returns></returns>
		RuntimeField GetField(ISignatureContext context, TokenTypes token);

		/// <summary>
		/// Retrieves the stackFrameIndex definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the _stackFrameIndex to retrieve.</param>
		/// <returns></returns>
		RuntimeField GetField( TokenTypes token);

		/// <summary>
		/// Retrieves the method definition identified by the given token in the scope.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="token">The token of the method to retrieve.</param>
		/// <returns></returns>
		RuntimeMethod GetMethod(ISignatureContext context, TokenTypes token);

		/// <summary>
		/// Retrieves the method definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the method to retrieve.</param>
		/// <returns></returns>
		RuntimeMethod GetMethod(TokenTypes token);

		/// <summary>
		/// Resolves the type of the signature.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="sigType">Type of the signature.</param>
		/// <returns></returns>
		RuntimeType ResolveSignatureType(ISignatureContext context, SigType sigType);

		/// <summary>
		/// Adds the internal compiler defined type to the type system
		/// </summary>
		/// <param name="type">The type.</param>
		void AddInternalType(RuntimeType type);
	}
}

