using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;

namespace Mosa.Runtime.TypeSystem
{
	public class TypeSystem : ITypeSystem
	{
		/// <summary>
		/// 
		/// </summary>
		private List<ITypeModule> typeModules = new List<ITypeModule>();

		/// <summary>
		/// Loads the module.
		/// </summary>
		/// <param name="module">The module.</param>
		void ITypeSystem.LoadModule(IMetadataModule metadataModule)
		{
			ITypeModule typeModule = new TypeModule(this, metadataModule);
			typeModules.Add(typeModule);
		}

		/// <summary>
		/// Gets the type modules.
		/// </summary>
		/// <value>The type modules.</value>
		IList<ITypeModule> ITypeSystem.TypeModules { get { return typeModules; } }

		/// <summary>
		/// Adds the module reference.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <returns></returns>
		ITypeModule ITypeSystem.ResolveModuleReference(string assembly)
		{
			// Search for reference first
			foreach (ITypeModule typeModule in typeModules)
			{
				if (typeModule.MetadataModule != null)
				{
					if (typeModule.MetadataModule.Name == assembly)
					{
						return typeModule; // already referenced
					}
				}
			}
			
			return null;
		}

		/// <summary>
		/// Gets the runtime type for the given type name and namespace
		/// </summary>
		/// <param name="nameSpace">The name space.</param>
		/// <param name="typeName">Name of the type.</param>
		/// <returns></returns>
		RuntimeType ITypeSystem.GetType(string nameSpace, string name)
		{
			foreach (ITypeModule typeModule in typeModules)
			{
				RuntimeType type = typeModule.GetType(nameSpace, name);
				if (type != null)
					return type;
			}

			return null;
		}

		
	}
}
