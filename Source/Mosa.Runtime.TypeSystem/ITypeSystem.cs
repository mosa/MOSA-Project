using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;

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
		/// Gets the type.
		/// </summary>
		/// <param name="fullname">The fullname.</param>
		/// <returns></returns>
		RuntimeType GetType(string fullname);
		
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
	}
}
