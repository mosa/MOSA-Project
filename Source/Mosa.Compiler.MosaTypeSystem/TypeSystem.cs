/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata.Loader;
using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class TypeSystem
	{
		public MosaTypeResolver Resolver { get; internal set; }

		public BuiltInTypes BuiltIn { get; internal set; }

		public IList<MosaType> AllTypes { get { return Resolver.Types; } }

		public IList<MosaAssembly> AllAssemblies { get { return Resolver.Assemblies; } }

		public TypeSystem()
		{
			Resolver = new MosaTypeResolver();
			this.BuiltIn = Resolver.BuiltIn;
		}

		public void LoadAssembly(IMetadataModule metadataModule)
		{
			MosaTypeLoader.Load(metadataModule, Resolver);
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


	}
}