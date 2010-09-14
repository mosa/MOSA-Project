/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Runtime;

namespace Mosa.Runtime.Vm
{
	/// <summary>
	/// A default implementation of a type system for the Mosa runtime.
	/// </summary>
	public sealed class DefaultTypeSystem : ITypeSystem
	{
		#region Data members

		/// <summary>
		/// Holds the metadata modules
		/// </summary>
		private List<IModuleTypeSystem> modules = new List<IModuleTypeSystem>();

		/// <summary>
		/// Holds the metadata modules which are to be compiled
		/// </summary>
		private List<IModuleTypeSystem> compileModules = new List<IModuleTypeSystem>();

		/// <summary>
		/// Holds the metadata module for internally created types
		/// </summary>
		private IModuleTypeSystem internalModuleTypeSystem;

		/// <summary>
		/// Holds the assembly loader
		/// </summary>
		private IAssemblyLoader assemblyLoader;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes static data members of the type loader.
		/// </summary>
		public DefaultTypeSystem(IAssemblyLoader assemblyLoader)
		{
			this.assemblyLoader = assemblyLoader;
		}

		#endregion // Construction

		#region ITypeSystem Members

		/// <summary>
		/// Loads the modules.
		/// </summary>
		/// <param name="files">The files.</param>
		void ITypeSystem.LoadModules(IEnumerable<string> files)
		{
			foreach (string file in files)
			{
				IMetadataModule metaModule = this.assemblyLoader.LoadModule(file);

				IModuleTypeSystem moduleTypeSystem = new DefaultModuleTypeSystem(this, metaModule);

				modules.Add(moduleTypeSystem);
				compileModules.Add(moduleTypeSystem);
			}
		}

		/// <summary>
		/// Adds the module reference.
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <returns></returns>
		IModuleTypeSystem ITypeSystem.ResolveModuleReference(string assembly)
		{
			// Search for reference first
			foreach (IModuleTypeSystem module in modules)
				if (module.MetadataModule != null)
					if (module.MetadataModule.Name == assembly)
						return module; // already referenced

			IMetadataModule metaModule = this.assemblyLoader.LoadModule(assembly);

			IModuleTypeSystem moduleTypeSystem = new DefaultModuleTypeSystem(this, metaModule);

			modules.Add(moduleTypeSystem);

			return moduleTypeSystem;
		}

		/// <summary>
		/// Gets the main module type system.
		/// </summary>
		/// <returns></returns>
		IModuleTypeSystem ITypeSystem.GetMainModuleTypeSystem()
		{
			return modules[0];
		}

		/// <summary>
		/// Retrieves the runtime type for a given type name.
		/// </summary>
		/// <param name="typeName">The name of the type to locate.</param>
		/// <returns>The located <see cref="RuntimeType"/> or null.</returns>
		RuntimeType ITypeSystem.GetType(string typeName)
		{
			string[] names = typeName.Split(',');

			if (names.Length > 1)
			{
				IModuleTypeSystem module = ((ITypeSystem)this).ResolveModuleReference(names[1].Trim());

				Debug.Assert(module != null);

				RuntimeType type = module.GetType(typeName);

				if (type != null)
					return type;

				// something went wrong
				Debug.Assert(false);
			}

			foreach (IModuleTypeSystem module in modules)
			{
				RuntimeType type = module.GetType(typeName);

				if (type != null)
					return type;
			}

			return null;
		}

		/// <summary>
		/// Gets the internal module type system.
		/// </summary>
		/// <value>The internal module type system.</value>
		IModuleTypeSystem ITypeSystem.InternalModuleTypeSystem
		{
			get
			{
				if (internalModuleTypeSystem == null)
				{
					internalModuleTypeSystem = new DefaultModuleTypeSystem(this);

					modules.Add(internalModuleTypeSystem);
					compileModules.Add(internalModuleTypeSystem);
				}

				return this.internalModuleTypeSystem;
			}
		}

		/// <summary>
		/// Retrieves the runtime type for a given type name.
		/// </summary>
		/// <returns>
		/// The located <see cref="RuntimeType"/> or null.
		/// </returns>
		IEnumerable<RuntimeType> ITypeSystem.GetCompiledTypes()
		{
			foreach (IModuleTypeSystem module in compileModules)
			{
				foreach (RuntimeType type in module.GetTypes())
				{
					yield return type;
				}
			}
		}

		/// <summary>
		/// Adds the internal compiler defined type to the type system
		/// </summary>
		/// <param name="type">The type.</param>
		void ITypeSystem.AddInternalType(RuntimeType type)
		{
			IModuleTypeSystem module = ((ITypeSystem)this).InternalModuleTypeSystem;

			module.AddInternalType(type);
		}

		#endregion // ITypeSystem Members

	}
}
