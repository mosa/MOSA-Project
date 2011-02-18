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
		/// Holds the type module for internally created types
		/// </summary>
		private InternalTypeModule internalTypeModule;

		/// <summary>
		/// Loads the module.
		/// </summary>
		/// <param name="modules">The modules.</param>
		void ITypeSystem.LoadModules(IList<IMetadataModule> modules)
		{
			foreach (IMetadataModule module in modules)
			{
				ITypeModule typeModule = new TypeModule(this, module);
				typeModules.Add(typeModule);
			}
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
		/// <param name="name">The name.</param>
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


		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <param name="fullname">The fullname.</param>
		/// <returns></returns>
		RuntimeType ITypeSystem.GetType(string fullname)
		{
			int dot = fullname.LastIndexOf(".");

			if (dot < 0)
				return null;

			return ((ITypeSystem)this).GetType(fullname.Substring(0, dot), fullname.Substring(dot + 1));
		}

		/// <summary>
		/// Gets all types from type system.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> ITypeSystem.GetAllTypes()
		{
			foreach (ITypeModule typeModule in typeModules)
			{
				foreach (RuntimeType type in typeModule.GetAllTypes())
				{
					if (type != null)
						yield return type;
				}
			}
		}

		/// <summary>
		/// Gets the internal type module.
		/// </summary>
		/// <value>The internal type module.</value>
		InternalTypeModule ITypeSystem.InternalTypeModule
		{
			get
			{
				if (internalTypeModule == null)
				{
					internalTypeModule = new InternalTypeModule(this);

					typeModules.Add(internalTypeModule);
				}

				return this.internalTypeModule;
			}
		}

		/// <summary>
		/// Adds the internal compiler defined type to the type system
		/// </summary>
		/// <param name="type">The type.</param>
		void ITypeSystem.AddInternalType(RuntimeType type)
		{
			((ITypeSystem)this).InternalTypeModule.AddType(type);
		}
	}
}
