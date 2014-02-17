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
		public MosaTypeResolver Resolver { get; private set; }

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

		public MosaMethod EntryPoint { get; private set; }

		private TypeSystem()
		{
			Resolver = new MosaTypeResolver();
		}

		static MosaModule LoadModule(MosaTypeLoader loader, ModuleDefMD module)
		{
			return loader.Load(module);
		}

		public static TypeSystem Load(MosaModuleLoader moduleLoader)
		{
			TypeSystem result = new TypeSystem();

			MosaTypeLoader loader = new MosaTypeLoader(result);
			foreach (var module in moduleLoader.Modules)
			{
				var mosaModule = LoadModule(loader, module);

				if (module.Assembly.IsCorLib())
				{
					result.BuiltIn = new BuiltInTypes(mosaModule);
					result.CorLib = mosaModule;
				}
			}

			if (result.BuiltIn == null)
				throw new AssemblyLoadException();

			loader.Resolve();

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
			if (this.Resolver.Modules.TryGetValue(name, out result))
				return result;

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
	}
}