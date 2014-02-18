/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System.Collections.Generic;
using dnlib.DotNet;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	///
	/// </summary>
	public class TypeSystem
	{
		internal MosaTypeResolver Resolver { get; private set; }
		MosaTypeLoader loader;

		public BuiltInTypes BuiltIn { get; private set; }

		public MosaModule CorLib { get; private set; }

		public IEnumerable<MosaType> AllTypes
		{
			get
			{
				foreach (var module in AllModules)
				{
					foreach (var type in module.Types.Values)
					{
						yield return type;
					}
				}
			}
		}

		public IEnumerable<MosaModule> AllModules { get { return Resolver.Modules.Values; } }

		public MosaType DefaultLinkerType { get { return Resolver.DefaultLinkerType; } }

		public MosaMethod EntryPoint { get; private set; }


		private TypeSystem()
		{
			Resolver = new MosaTypeResolver();
			loader = new MosaTypeLoader(this);
		}

		static MosaModule LoadModule(MosaTypeLoader loader, ModuleDefMD module)
		{
			return loader.Load(module);
		}

		public static TypeSystem Load(MosaModuleLoader moduleLoader)
		{
			TypeSystem result = new TypeSystem();

			foreach (var module in moduleLoader.Modules)
			{
				var mosaModule = LoadModule(result.loader, module);

				if (module.Assembly.IsCorLib())
				{
					result.CorLib = mosaModule;
					result.BuiltIn = new BuiltInTypes(result, mosaModule);
				}
			}

			if (result.BuiltIn == null)
				throw new AssemblyLoadException();

			result.loader.Resolve();

			foreach (var module in result.AllModules)
			{
				if (module.EntryPoint != null)
					result.EntryPoint = module.EntryPoint;
			}

			return result;
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
			var typeKey = Tuple.Create(@namespace, @namespace + "." + name);

			foreach (var module in AllModules)
			{
				MosaType result;
				if (module.Types.TryGetValue(typeKey, out result))
					return result;
			}

			return null;
		}

		public MosaType GetTypeByName(string moduleName, string @namespace, string name)
		{
			var module = GetModuleByName(moduleName);

			if (module == null)
				return null;

			return GetTypeByName(module, @namespace, name);
		}

		public MosaType GetTypeByName(MosaModule module, string @namespace, string name)
		{
			MosaType result;
			if (module.Types.TryGetValue(Tuple.Create(@namespace, @namespace + "." + name), out result))
				return result;

			return null;
		}

		public MosaModule GetModuleByName(string name)
		{
			MosaModule result;
			if (Resolver.Modules.TryGetValue(name, out result))
				return result;

			return null;
		}

		public MosaModule GetModuleByAssembly(string name)
		{
			foreach (var module in AllModules)
			{
				if (module.InternalModule.Assembly.Name == name)
					return module;
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

		public uint LookupUserString(MosaModule module, string str)
		{
			return Resolver.stringHeapLookup[Tuple.Create(module.InternalModule, str)];
		}

		public MosaType GetUnmanagedPointerType(MosaType element)
		{
			return loader.GetType(new PtrSig(element.TypeSignature));
		}

		public MosaType GetManagedPointerType(MosaType element)
		{
			return loader.GetType(new ByRefSig(element.TypeSignature));
		}

		public MosaType GetArrayType(MosaType element)
		{
			return loader.GetType(new SZArraySig(element.TypeSignature));
		}

		public MosaType CreateNewElementType(MosaType baseType, MosaType elementType)
		{
			if (baseType.IsArray)
			{
				return GetArrayType(elementType);
			}
			else if (baseType.IsUnmanagedPointer)
			{
				return GetUnmanagedPointerType(elementType);
			}
			else if (baseType.IsManagedPointer)
			{
				return GetManagedPointerType(elementType);
			}

			throw new InvalidCompilerException();
		}

		public MosaType GetFunctionPointerType(MosaMethod method)
		{
			return loader.GetType(new FnPtrSig(method.MethodSignature));
		}

		public MosaType GetStackType(MosaType type)
		{
			switch (type.GetStackType())
			{
				case StackTypeCode.Int32:
					return BuiltIn.I4;

				case StackTypeCode.Int64:
					return BuiltIn.I8;

				case StackTypeCode.N:
					return BuiltIn.I;

				case StackTypeCode.F:
					return BuiltIn.R8;

				case StackTypeCode.O:
					return type;

				case StackTypeCode.UnmanagedPointer:
				case StackTypeCode.ManagedPointer:
					return type;
			}
			throw new CompilerException("Can't convert '" + type.FullName + "' to stack type.");
		}

		public MosaType GetStackTypeFromCode(StackTypeCode code)
		{
			switch (code)
			{
				case StackTypeCode.Int32:
					return BuiltIn.I4;

				case StackTypeCode.Int64:
					return BuiltIn.I8;

				case StackTypeCode.N:
					return BuiltIn.I;

				case StackTypeCode.F:
					return BuiltIn.R8;

				case StackTypeCode.O:
					return BuiltIn.Object;

				case StackTypeCode.UnmanagedPointer:
					return BuiltIn.Pointer;

				case StackTypeCode.ManagedPointer:
					return GetManagedPointerType(BuiltIn.Object);
			}
			throw new CompilerException("Can't convert stack type code'" + code + "' to type.");
		}

		public MosaType GetTypeFromElementCode(ElementType type)
		{
			switch (type)
			{
				case ElementType.Void: return BuiltIn.Void;
				case ElementType.Boolean: return BuiltIn.Boolean;
				case ElementType.Char: return BuiltIn.Char;
				case ElementType.I1: return BuiltIn.I1;
				case ElementType.U1: return BuiltIn.U1;
				case ElementType.I2: return BuiltIn.I2;
				case ElementType.U2: return BuiltIn.U2;
				case ElementType.I4: return BuiltIn.I4;
				case ElementType.U4: return BuiltIn.U4;
				case ElementType.I8: return BuiltIn.I8;
				case ElementType.U8: return BuiltIn.U8;
				case ElementType.R4: return BuiltIn.R4;
				case ElementType.R8: return BuiltIn.R8;
				case ElementType.I: return BuiltIn.I;
				case ElementType.U: return BuiltIn.U;
				case ElementType.String: return BuiltIn.String;
				case ElementType.TypedByRef: return BuiltIn.TypedRef;
				case ElementType.Object: return BuiltIn.Object;
			}
			throw new CompilerException("Can't convert element '" + type + "' to type.");
		}
	}
}