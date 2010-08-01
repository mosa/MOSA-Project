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
		private List<IMetadataModule> loadedImages = new List<IMetadataModule>();
		private object loaderLock = new object();
		private RuntimeBase runtimeBase;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
		/// </summary>
		/// <param name="runtimeBase">The runtime base.</param>
		public AssemblyLoader(RuntimeBase runtimeBase)
		{
			this.runtimeBase = runtimeBase;

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
			get { return loadedImages; }
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
				return loadedImages[index];
			}
		}

		/// <summary>
		/// Resolves the given assembly reference and loads the associated IMetadataModule.
		/// </summary>
		/// <param name="provider">The metadata provider, which contained the assembly reference.</param>
		/// <param name="assemblyRef">The assembly reference to resolve.</param>
		/// <returns>
		/// An instance of IMetadataModule representing the resolved assembly.
		/// </returns>
		IMetadataModule IAssemblyLoader.Resolve(IMetadataProvider provider, AssemblyRefRow assemblyRef)
		{
			string name = provider.ReadString(assemblyRef.NameIdx);

			IMetadataModule result = GetLoadedAssembly(name) ?? DoLoadAssembly(name + ".dll");
			if (result == null)
				throw new TypeLoadException();

			return result;
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
			IMetadataModule result;

			if (Path.IsPathRooted(file) == false)
			{
				result = GetLoadedAssembly(file);
				if (result == null)
				{
					result = DoLoadAssembly(Path.GetFileName(file) + @".dll");
				}
			}
			else
			{
				result = LoadAssembly(file);
			}

			return result;
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

		/// <summary>
		/// Loads the assembly.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		private IMetadataModule LoadAssembly(string file)
		{
			IMetadataModule result;
			string codeBase = CreateFileCodeBase(file);

			result = FindLoadedModule(codeBase);
			if (result == null)
			{
				if (!File.Exists(file))
					return null;

				lock (loaderLock)
				{
					result = PortableExecutableImage.Load(loadedImages.Count, new FileStream(file, FileMode.Open, FileAccess.Read), codeBase);
					if (result != null)
					{
						loadedImages.Add(result);

						runtimeBase.TypeSystem.AssemblyLoaded(result);
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Finds the loaded module.
		/// </summary>
		/// <param name="codeBase">The code base.</param>
		/// <returns></returns>
		private IMetadataModule FindLoadedModule(string codeBase)
		{
			foreach (IMetadataModule module in loadedImages)
			{
				if (module.CodeBase.Equals(codeBase))
				{
					return module;
				}
			}

			return null;
		}

		private string CreateFileCodeBase(string file)
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
			IMetadataModule result = null;

			lock (loaderLock)
			{
				foreach (IMetadataModule image in loadedImages)
				{
					if (name.Equals(image.Name))
					{
						result = image;
						break;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Does the load assembly.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		private IMetadataModule DoLoadAssembly(string name)
		{
			IMetadataModule result = null;

			lock (loaderLock)
			{
				result = DoLoadAssemblyFromPaths(name, privatePaths) ?? DoLoadAssemblyFromPaths(name, searchPath);
			}

			return result;
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