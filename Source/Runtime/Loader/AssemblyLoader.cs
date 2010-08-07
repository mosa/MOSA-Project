/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.IO;
using Mosa.Runtime.Loader.PE;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Loader
{
	/// <summary>
	/// Provides a default implementation of the IAssemblyLoader interface.
	/// </summary>
	public class AssemblyLoader : IAssemblyLoader
	{
		#region Data members

		private string[] searchPath;
		private List<string> privatePaths = new List<string>();
		private List<IMetadataModule> modules = new List<IMetadataModule>();
		private object loaderLock = new object();
		private ITypeSystem typeSystem;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
		/// </summary>
		/// <param name="baseRuntime">The runtime base.</param>
		public AssemblyLoader(ITypeSystem typeSystem)
		{
			this.typeSystem = typeSystem;

			// HACK: I can't figure out an easier way to get the framework dir right now...
			string frameworkDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
			string frameworkDir32 = frameworkDir.Contains("Framework64") ? frameworkDir.Replace("Framework64", "Framework") : frameworkDir;
			searchPath = new[] {
				AppDomain.CurrentDomain.BaseDirectory,
				frameworkDir,
				frameworkDir32
			};

		}

		#endregion // Construction

		#region IAssemblyLoader

		/// <summary>
		/// Gets an enumerable collection of loaded modules.
		/// </summary>
		/// <value></value>
		public IEnumerable<IMetadataModule> Modules
		{
			get { return modules; }
		}

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		void IAssemblyLoader.AppendPrivatePath(string path)
		{
			lock (loaderLock)
			{
				privatePaths.Add(path);
			}
		}

		/// <summary>
		/// Gets the module.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		IMetadataModule IAssemblyLoader.GetModule(int index)
		{
			lock (loaderLock)
			{
				return modules[index];
			}
		}

		/// <summary>
		/// Loads the named assembly.
		/// </summary>
		/// <param name="file">The file path of the assembly to load.</param>
		/// <returns>
		/// The assembly image of the loaded assembly.
		/// </returns>
		IMetadataModule IAssemblyLoader.Load(string file)
		{
			lock (loaderLock)
			{
				IMetadataModule module = LoadAssembly(file);

				if (module != null)
				{

					if (module.LoadOrder < 0)
					{
						module.LoadOrder = modules.Count;
						modules.Add(module);
						typeSystem.AssemblyLoaded(module);
					}
				}

				return module;
			}
		}

		/// <summary>
		/// Loads the named assemblies (as a merged assembly)
		/// </summary>
		/// <param name="files"></param>
		/// <returns>
		/// The assembly image of the loaded assembly.
		/// </returns>
		IMetadataModule IAssemblyLoader.MergeLoad(IEnumerable<string> files)
		{
			lock (loaderLock)
			{
				List<IMetadataModule> mods = new List<IMetadataModule>();

				foreach (string file in files)
				{
					IMetadataModule result = DoLoadAssembly(file);

					if (result != null)
					{
						mods.Add(result);
					}
				}

				IMetadataModule merged = new MergedMetadata(mods);

				merged.LoadOrder = modules.Count;
				modules.Add(merged);
				typeSystem.AssemblyLoaded(merged);

				return merged;
			}
		}
		/// <summary>
		/// Unloads the given module.
		/// </summary>
		/// <param name="module">The module to unload.</param>
		void IAssemblyLoader.Unload(IMetadataModule module)
		{
			IDisposable disp = module as IDisposable;
			if (null != disp)
			{
				disp.Dispose();
			}
		}

		#endregion // IAssemblyLoader

		#region Internals

		private IMetadataModule LoadAssembly(string file)
		{
			IMetadataModule result = null;

			if (!Path.IsPathRooted(file))
			{
				result = GetLoadedAssembly(file);

				if (result == null)
				{
					result = DoLoadAssembly(Path.GetFileName(file) + @".dll");
				}
			}
			else
			{
				result = LoadAssembly2(file);
			}

			return result;
		}

		/// <summary>
		/// Loads the assembly.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		private IMetadataModule LoadAssembly2(string file)
		{
			string codeBase = CreateFileCodeBase(file);

			IMetadataModule result = FindLoadedModule(codeBase);

			if (result != null)
				return result;

			if (!File.Exists(file))
				return null;

			return PortableExecutableImage.Load(new FileStream(file, FileMode.Open, FileAccess.Read), codeBase);
		}


		/// <summary>
		/// Finds the loaded module.
		/// </summary>
		/// <param name="codeBase">The code base.</param>
		/// <returns></returns>
		private IMetadataModule FindLoadedModule(string codeBase)
		{
			foreach (IMetadataModule module in modules)
			{
				foreach (string code in module.CodeBases)
				{
					if (code.Equals(codeBase))
					{
						return module;
					}
				}
			}

			return null;
		}

		private static string CreateFileCodeBase(string file)
		{
			return @"file://" + file.Replace('\\', '/');
		}

		/// <summary>
		/// Gets the loaded assembly.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private IMetadataModule GetLoadedAssembly(string name)
		{
			foreach (IMetadataModule module in modules)
			{
				foreach (string modname in module.Names)
				{
					if (name.Equals(modname))
					{
						return module;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Does the load assembly.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private IMetadataModule DoLoadAssembly(string name)
		{
			return DoLoadAssemblyFromPaths(name, privatePaths) ?? DoLoadAssemblyFromPaths(name, searchPath);
		}

		/// <summary>
		/// Does the load assembly From paths.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="paths">The paths.</param>
		/// <returns></returns>
		private IMetadataModule DoLoadAssemblyFromPaths(string name, IEnumerable<string> paths)
		{
			IMetadataModule result = null;
			string fullName;

			foreach (string path in paths)
			{
				fullName = Path.Combine(path, name);
				try
				{
					result = LoadAssembly(fullName);
					if (result != null)
						break;
				}
				catch
				{
					/* Failed to load assembly there... */
				}
			}

			return result;
		}

		#endregion // Internals
	}
}