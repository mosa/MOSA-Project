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

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem.Generic;

namespace Mosa.Runtime.TypeSystem
{
	public interface ITypeSystem
	{

		/// <summary>
		/// Loads the modules.
		/// </summary>
		/// <param name="modules">The modules.</param>
		void LoadModules(IList<IMetadataModule> modules);

		/// <summary>
		/// Gets the type modules.
		/// </summary>
		/// <value>The type modules.</value>
		IList<ITypeModule> TypeModules { get; }

		/// <summary>
		/// Gets the runtime type for the given type name and namespace
		/// </summary>
		/// <param name="nameSpace">The name space.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		RuntimeType GetType(string nameSpace, string name);

		/// <summary>
		/// Gets the runtime type for the given type name and namespace
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <param name="nameSpace">The name space.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		RuntimeType GetType(string assembly, string nameSpace, string name);

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		RuntimeType GetType(string name);

		/// <summary>
		/// Resolves the module reference.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <returns></returns>
		ITypeModule ResolveModuleReference(string assembly);

		/// <summary>
		/// Gets all types from type system.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> GetAllTypes();

		/// <summary>
		/// Gets the internal type module.
		/// </summary>
		/// <value>The internal type module.</value>
		ITypeModule InternalTypeModule { get; }

		/// <summary>
		/// Gets the generic type patcher.
		/// </summary>
		IGenericTypePatcher GenericTypePatcher { get; }

		/// <summary>
		/// Adds the internal compiler defined type to the type system
		/// </summary>
		/// <param name="type">The type.</param>
		void AddInternalType(RuntimeType type);

		/// <summary>
		/// Gets the main type module.
		/// </summary>
		/// <returns></returns>
		ITypeModule MainTypeModule { get; set; }

		/// <summary>
		/// Gets the open generic.
		/// </summary>
		/// <param name="baseGenericType">Type of the base generic.</param>
		/// <returns></returns>
		CilGenericType GetOpenGeneric(RuntimeType baseGenericType);

		/// <summary>
		/// Resolves the type of the generic.
		/// </summary>
		/// <param name="typeModule">The type module.</param>
		/// <param name="typeSpecSignature">The type spec signature.</param>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		RuntimeType ResolveGenericType(ITypeModule typeModule, TypeSpecSignature typeSpecSignature, Token token);
	}
}
