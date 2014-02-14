/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Metadata.Loader;
using System;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	///
	/// </summary>
	public class TypeSystem
	{
		public MosaTypeResolver Resolver { get; internal set; }

		public BuiltInTypes BuiltIn { get; internal set; }

		public MosaMethod StartupMethod { get; internal set; }

		public IList<MosaType> AllTypes { get { return Resolver.Types; } }

		public IList<MosaAssembly> AllAssemblies { get { return Resolver.Assemblies; } }

		public MosaMethod EntryMethod { get; private set; }

		public TypeSystem()
		{
			Resolver = new MosaTypeResolver();
			this.BuiltIn = Resolver.BuiltIn;
		}

		public void LoadAssembly(IMetadataModule metadataModule)
		{
			var entryMethod = MosaTypeLoader.Load(metadataModule, Resolver);

			if (entryMethod != null)
			{
				EntryMethod = entryMethod;
			}
		}

		public void Load(MosaAssemblyLoader assemblyLoader)
		{
			foreach (var module in assemblyLoader.Modules)
			{
				LoadAssembly(module);
			}
		}

		public MosaType GetTypeByName(string fullname)
		{
			int dot = fullname.LastIndexOf(".");

			if (dot < 0)
				return null;

			return GetTypeByName(fullname.Substring(0, dot), fullname.Substring(dot + 1));
		}

		public MosaType GetTypeByName(string @namespace, string name)
		{
			foreach (var type in AllTypes)
			{
				if (type.Name == name && type.Namespace == @namespace)
					return type;
			}

			return null;
		}

		public MosaType GetTypeByName(string assemblyName, string @namespace, string name)
		{
			var assembly = GetAssemblyByName(assemblyName);

			if (assemblyName == null)
				return null;

			return GetTypeByName(assembly, @namespace, name);
		}

		public MosaType GetTypeByName(MosaAssembly assembly, string @namespace, string name)
		{
			foreach (var type in AllTypes)
			{
				if (type.Name == name && type.Namespace == @namespace && type.Assembly == assembly)
					return type;
			}

			return null;
		}

		public MosaAssembly GetAssemblyByName(string name)
		{
			foreach (var assembly in this.Resolver.Assemblies)
			{
				if (assembly.Name == name)
					return assembly;
			}

			return null;
		}

		public static MosaMethod GetMethodByName(MosaType type, string name)
		{
			foreach (var method in type.Methods)
			{
				if (method.Name == name)
					return method;
			}

			return null;
		}

		public static MosaMethod GetMethodByNameAndParameters(MosaType type, string name, IList<MosaParameter> parameters)
		{
			foreach (var method in type.Methods)
			{
				if (method.Name != name)
					continue;

				if (method.Parameters.Count != parameters.Count)
					continue;

				bool match = true;
				for (int i = 0; i < method.Parameters.Count; i++)
				{
					if (!method.Parameters[i].Type.Matches(parameters[i].Type))
					{
						match = false;
						break;
					}
				}

				if (match)
					return method;
			}

			return null;
		}

		/// <summary>
		/// Converts the type of to stack.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public MosaType ConvertToStackType(MosaType type)
		{
			// FIXME! This is 32-bit platform specific ---
			if (type.IsNativeInteger || type.IsByte || type.IsShort || type.IsChar || type.IsBoolean)
				return BuiltIn.I4;
			else
				return type;
		}

		/// <summary>
		/// Creates the type of the linker.
		/// </summary>
		/// <param name="namespace">The namespace.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public MosaType CreateLinkerType(string @namespace, string name)
		{
			return Resolver.CreateLinkerType(@namespace, name);
		}

		/// <summary>
		/// Creates the linker method.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		/// <param name="returnType">Type of the return.</param>
		/// <returns></returns>
		public MosaMethod CreateLinkerMethod(MosaType declaringType, string name, MosaType returnType, IList<MosaType> parameters)
		{
			return Resolver.CreateLinkerMethod(declaringType, name, returnType, parameters);
		}

		/// <summary>
		/// Creates the linker method.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="returnType">Type of the return.</param>
		/// <returns></returns>
		public MosaMethod CreateLinkerMethod(string name, MosaType returnType, IList<MosaType> parameters)
		{
			return Resolver.CreateLinkerMethod(Resolver.DefaultLinkerType, name, returnType, parameters);
		}

		/// <summary>
		/// Gets the type on the stack.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>
		/// The equivalent stack type code.
		/// </returns>
		/// <exception cref="InvalidCompilerException"></exception>
		public static StackTypeCode GetStackType(MosaType type)
		{
			if (type.IsLong) return StackTypeCode.Int64;
			else if (type.IsInteger || type.IsChar || type.IsBoolean) return StackTypeCode.Int32;
			else if (type.IsFloatingPoint) return StackTypeCode.F;
			else if (type.IsUnmanagedPointerType) return StackTypeCode.Ptr;
			else if (type.IsManagedPointerType) return StackTypeCode.O;
			else if (type.IsNativeInteger) return StackTypeCode.N;
			else if (type.IsObject || type.IsValueType || type.IsArray || type.IsString) return StackTypeCode.O;
			else if (type.IsVoid) return StackTypeCode.Unknown;

			throw new InvalidCompilerException(String.Format("Can't transform Type {0} to StackTypeCode.", type));
		}

		public MosaType GetType(StackTypeCode typeCode)
		{
			switch (typeCode)
			{
				case StackTypeCode.Int32: return BuiltIn.I4;
				case StackTypeCode.Int64: return BuiltIn.I8;
				case StackTypeCode.F: return BuiltIn.R8;
				case StackTypeCode.O: return BuiltIn.Object;
				case StackTypeCode.N: return BuiltIn.I;
				default: throw new InvalidCompilerException("Can't convert stack type codeReader to type.");
			}
		}
	}
}