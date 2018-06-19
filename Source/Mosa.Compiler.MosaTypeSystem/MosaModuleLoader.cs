﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;
using Mosa.Compiler.MosaTypeSystem.Metadata;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaModuleLoader : IDisposable
	{
		public AssemblyResolver Resolver { get; }

		internal List<ModuleDefMD> Modules { get; }

		public MosaModuleLoader()
		{
			Modules = new List<ModuleDefMD>();
			Resolver = new AssemblyResolver(null, false) { UseGAC = false };
			var typeResolver = new Resolver(Resolver);
			Resolver.DefaultModuleContext = new ModuleContext(Resolver, typeResolver);
			Resolver.EnableTypeDefCache = true;
		}

		#region Internal methods

		private List<string> seenModules = new List<string>();

		private void LoadDependencies(ModuleDefMD module)
		{
			if (seenModules.Contains(module.Location))
				return;

			seenModules.Add(module.Location);
			Modules.Add(module);
			Resolver.AddToCache(module);

			foreach (var assemblyRef in module.GetAssemblyRefs())
			{
				var assembly = Resolver.ResolveThrow(assemblyRef, null);

				foreach (var moduleRef in assembly.Modules)
				{
					LoadDependencies((ModuleDefMD)moduleRef);
				}
			}
		}

		#endregion Internal methods

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		public void AddPrivatePath(string path)
		{
			if (!Resolver.PostSearchPaths.Contains(path))
			{
				Resolver.PostSearchPaths.Add(path);
			}
		}

		/// <summary>
		/// Appends the given paths to the assembly search path.
		/// </summary>
		/// <param name="assemblyPaths">The assembly paths.</param>
		public void AddPrivatePath(IEnumerable<string> assemblyPaths)
		{
			foreach (string path in assemblyPaths)
			{
				AddPrivatePath(Path.GetDirectoryName(path));
			}
		}

		/// <summary>
		/// Loads the module.
		/// </summary>
		/// <param name="file">The file path of the module to load.</param>
		public void LoadModuleFromFile(string file)
		{
			var module = ModuleDefMD.Load(file, Resolver.DefaultModuleContext);
			module.EnableTypeDefFindCache = true;

			LoadDependencies(module);
		}

		public IMetadata CreateMetadata()
		{
			return new CLRMetadata(this);
		}

		public void Dispose()
		{
			foreach (var module in Modules)
			{
				module.Dispose();
			}

			Modules.Clear();
		}
	}
}
