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
using System.Diagnostics;

using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem.Generic;

namespace Mosa.Compiler.TypeSystem
{
	public sealed class TypeSystem : ITypeSystem
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
		/// Holds the main type module
		/// </summary>
		private ITypeModule mainTypeModule;

		#region ITypeSystem interface

		/// <summary>
		/// Loads the module.
		/// </summary>
		/// <param name="modules">The modules.</param>
		void ITypeSystem.LoadModules(IList<IMetadataModule> modules)
		{
			foreach (var module in modules)
			{
				ITypeModule typeModule = new TypeModule(this, module);
				typeModules.Add(typeModule);

				if (typeModule.MetadataModule.ModuleType == ModuleType.Executable)
					mainTypeModule = typeModule;
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
			return typeModules.Find(typeModule => typeModule.Name == assembly);
		}

		/// <summary>
		/// Gets the runtime type for the given type name and namespace
		/// </summary>
		/// <param name="nameSpace">The name space.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		RuntimeType ITypeSystem.GetType(string nameSpace, string name)
		{
			foreach (var typeModule in typeModules)
			{
				var type = typeModule.GetType(nameSpace, name);
				if (type != null)
					return type;
			}

			return null;
		}

		/// <summary>
		/// Gets the runtime type for the given type name and namespace
		/// </summary>
		/// <param name="assembly">The assembly.</param>
		/// <param name="nameSpace">The name space.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		RuntimeType ITypeSystem.GetType(string assembly, string nameSpace, string name)
		{
			var module = ((ITypeSystem)this).ResolveModuleReference(assembly);

			if (module == null)
				return null;

			return module.GetType(nameSpace, name);
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		RuntimeType ITypeSystem.GetType(string name)
		{
			Debug.Assert(name.IndexOf(',') < 0);

			int dot = name.LastIndexOf('.');

			if (dot < 0)
				return null;

			return (this as ITypeSystem).GetType(name.Substring(0, dot), name.Substring(dot + 1));
		}

		/// <summary>
		/// Gets all types from type system.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> ITypeSystem.GetAllTypes()
		{
			foreach (var typeModule in typeModules)
			{
				foreach (var type in typeModule.GetAllTypes())
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
		ITypeModule ITypeSystem.InternalTypeModule
		{
			get
			{
				InitializeInternalTypeModule();

				return this.internalTypeModule;
			}
		}

		/// <summary>
		/// Adds the internal compiler defined type to the type system
		/// </summary>
		/// <param name="type">The type.</param>
		void ITypeSystem.AddInternalType(RuntimeType type)
		{
			InitializeInternalTypeModule();
			internalTypeModule.AddType(type);
		}

		/// <summary>
		/// Gets the main type module.
		/// </summary>
		/// <returns></returns>
		ITypeModule ITypeSystem.MainTypeModule
		{
			get { return mainTypeModule; }
			set { mainTypeModule = value; }
		}

		/// <summary>
		/// Gets the open generic.
		/// </summary>
		/// <param name="baseGenericType">Type of the base generic.</param>
		/// <returns></returns>
		CilGenericType ITypeSystem.GetOpenGeneric(RuntimeType baseGenericType)
		{
			foreach (var typeModule in typeModules)
			{
				var type = typeModule.GetOpenGeneric(baseGenericType);

				if (type != null)
				{
					return type;
				}
			}

			return null;
		}

		RuntimeType ITypeSystem.ResolveGenericType(ITypeModule typeModule, TypeSpecSignature typeSpecSignature, Token token)
		{
			var genericInstSigType = typeSpecSignature.Type as GenericInstSigType;

			if (genericInstSigType == null)
				return null;

			RuntimeType genericType = null;
			SigType sigType = genericInstSigType;

			switch (genericInstSigType.Type)
			{
				case CilElementType.ValueType:
					goto case CilElementType.Class;

				case CilElementType.Class:
					TypeSigType typeSigType = (TypeSigType)sigType;
					genericType = typeModule.GetType(typeSigType.Token);
					break;

				case CilElementType.GenericInst:
					var genericBaseType = typeModule.GetType(genericInstSigType.BaseType.Token);
					genericType = new CilGenericType(typeModule, token, genericBaseType, genericInstSigType);
					break;

				default:
					throw new NotSupportedException(String.Format(@"LoadTypeSpecs does not support CilElementType.{0}", genericInstSigType.Type));
			}

			return genericType;
		}

		#endregion // ITypeSystem interface

		/// <summary>
		/// Initializes the internal type module.
		/// </summary>
		private void InitializeInternalTypeModule()
		{
			if (internalTypeModule == null)
			{
				internalTypeModule = new InternalTypeModule(this);
				typeModules.Add(internalTypeModule);
			}
		}
	}
}
