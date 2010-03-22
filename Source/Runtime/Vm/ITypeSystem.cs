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

namespace Mosa.Runtime.Vm
{
	using Mosa.Runtime.Metadata.Signatures;
	
    /// <summary>
    /// The interface of the type system.
    /// </summary>
    /// <remarks>
    /// The type system is responsible for loading assembly metadata and building
    /// runtime accessible management structures from those.
    /// </remarks>
    public interface ITypeSystem
    {
        #region Properties

        /// <summary>
        /// Returns an array of all fields loaded in the type system.
        /// </summary>
        RuntimeField[] Fields { get; }

        /// <summary>
        /// Returns an array of all methods in the type system.
        /// </summary>
        RuntimeMethod[] Methods { get; }

        /// <summary>
        /// Returns an array of all parameters in the type system.
        /// </summary>
        RuntimeParameter[] Parameters { get; }

        /// <summary>
        /// Returns an array of all types in the type system.
        /// </summary>
        RuntimeType[] Types { get; }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Notifies the type system that a CIL module was loaded.
        /// </summary>
        /// <param name="module">The loaded module.</param>
        void AssemblyLoaded(IMetadataModule module);

        /// <summary>
        /// Gets the types from module.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns></returns>
        ReadOnlyRuntimeTypeListView GetTypesFromModule(IMetadataModule module);

        /// <summary>
        /// Gets the module offset.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <returns></returns>
        ModuleOffsets GetModuleOffset(IMetadataModule module);

        /// <summary>
        /// Finds the type index from token.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        int FindTypeIndexFromToken(IMetadataModule module, TokenTypes token);

        /// <summary>
        /// Retrieves the runtime type for a given metadata token.
        /// </summary>
        /// <param name="module">The module, which owns the token.</param>
        /// <param name="token">The token of the type to load. This can represent a typeref, typedef or typespec token.</param>
        /// <returns>The runtime type of the specified token.</returns>
        RuntimeType GetType(ISignatureContext context, IMetadataModule module, TokenTypes token);

        /// <summary>
        /// Retrieves the runtime type for a given type name.
        /// </summary>
        /// <param name="typeName">The name of the type to locate.</param>
        /// <returns>The located <see cref="RuntimeType"/> or null.</returns>
        RuntimeType GetType(string typeName);

        /// <summary>
        /// Retrieves the _stackFrameIndex definition identified by the given token in the scope.
        /// </summary>
        /// <param name="context">The generic parameter resolution context.</param>
        /// <param name="module">The module of the token definition.</param>
        /// <param name="token">The token of the _stackFrameIndex to retrieve.</param>
        RuntimeField GetField(ISignatureContext context, IMetadataModule module, TokenTypes token);

        /// <summary>
        /// Retrieves the method definition identified by the given token in the scope.
        /// </summary>
        /// <param name="module">The scope of the token definition.</param>
        /// <param name="token">The token of the method to retrieve.</param>
        RuntimeMethod GetMethod(ISignatureContext context, IMetadataModule module, TokenTypes token);

        /// <summary>
        /// Retrieves the method for the named internal call.
        /// </summary>
        /// <param name="internalMethod">The internal method to retrieve the implementation for.</param>
        /// <returns>The method currently represented by the internal call.</returns>
        RuntimeMethod GetImplementationForInternalCall(RuntimeMethod internalMethod);

        RuntimeType ResolveSignatureType(ISignatureContext context, IMetadataModule module, SigType sigType);

        #endregion // Methods
    }
}
