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

		private string[] _searchPath;
		private List<string> _privatePaths = new List<string>();
		private List<IMetadataModule> _loadedImages = new List<IMetadataModule>();
		private ITypeSystem _typeLoader;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
		/// </summary>
		/// <param name="typeLoader">The type loader.</param>
		public AssemblyLoader(ITypeSystem typeLoader)
		{
			// HACK: I can't figure out an easier way to get the framework dir right now...
            string frameworkDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            string frameworkDir32 = frameworkDir.Contains("Framework64") ? frameworkDir.Replace("Framework64", "Framework") : frameworkDir;
			_searchPath = new[] {
                AppDomain.CurrentDomain.BaseDirectory,
                frameworkDir,
                frameworkDir32
            };

			_typeLoader = typeLoader;
		}

		#endregion // Construction

		#region IAssemblyLoader Members

		/// <summary>
		/// Gets an enumerable collection of loaded modules.
		/// </summary>
		/// <value></value>
		public IEnumerable<IMetadataModule> Modules
		{
			get { return _loadedImages; }
		}

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		public void AppendPrivatePath(string path)
		{
			_privatePaths.Add(path);
		}

        public IMetadataModule GetModule(int loadIndex)
        {
            return this._loadedImages[loadIndex];
        }

		/// <summary>
		/// Resolves the given assembly reference and loads the associated IMetadataModule.
		/// </summary>
		/// <param name="provider">The metadata provider, which contained the assembly reference.</param>
		/// <param name="assemblyRef">The assembly reference to resolve.</param>
		/// <returns>
		/// An instance of IMetadataModule representing the resolved assembly.
		/// </returns>
		public IMetadataModule Resolve(IMetadataProvider provider, AssemblyRefRow assemblyRef)
		{
		    string name;
			provider.Read(assemblyRef.NameIdx, out name);

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
		public IMetadataModule Load(string file)
		{
		    if (Path.IsPathRooted(file) == false)
				return DoLoadAssembly(Path.GetFileName(file));
		    return LoadAssembly(file);
		}

	    /// <summary>
		/// Unloads the given module.
		/// </summary>
		/// <param name="module">The module to unload.</param>
		public void Unload(IMetadataModule module)
		{
			IDisposable disp = module as IDisposable;
			if (null != disp)
				disp.Dispose();
		}

		#endregion // IAssemblyLoader Members

		#region Internals

		private IMetadataModule LoadAssembly(string file)
		{
			if (!File.Exists(file))
				return null;

			IMetadataModule result = PortableExecutableImage.Load(_loadedImages.Count, new FileStream(file, FileMode.Open, FileAccess.Read));
			if (null != result) {
				lock (_loadedImages) {
					_loadedImages.Add(result);
				}

				_typeLoader.AssemblyLoaded(result);
			}

			return result;
		}

		private IMetadataModule GetLoadedAssembly(string name)
		{
			IMetadataModule result = null;
			foreach (IMetadataModule image in _loadedImages) {
				if (name.Equals(image.Name)) {
					result = image;
					break;
				}
			}
			return result;
		}

		private IMetadataModule DoLoadAssembly(string name)
		{
			IMetadataModule result = DoLoadAssemblyFromPaths(name, _privatePaths) ?? DoLoadAssemblyFromPaths(name, _searchPath);

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

			foreach (string path in paths) {
				fullName = Path.Combine(path, name);
				try {
					result = LoadAssembly(fullName);
					if (result != null)
						break;
				}
				catch {
					/* Failed to load assembly there... */
				}
			}

			return result;
		}

		#endregion // Internals
	}
}
