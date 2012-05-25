/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mosa.Compiler.Metadata.Loader.PE;

namespace Mosa.Compiler.Metadata.Loader
{
	/// <summary>
	/// Provides a default implementation of the IAssemblyLoader interface.
	/// </summary>
	public class AssemblyLoader : IAssemblyLoader
	{
		#region Data members

		private List<string> privatePaths = new List<string>();
		private object loaderLock = new object();

		private List<IMetadataModule> modules = new List<IMetadataModule>();

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
		/// </summary>
		public AssemblyLoader()
		{
		}

		#endregion // Construction

		#region IAssemblyLoader

		/// <summary>
		/// Loads the named assembly.
		/// </summary>
		/// <param name="file">The file path of the assembly to load.</param>
		/// <returns>
		/// The assembly image of the loaded assembly.
		/// </returns>
		IMetadataModule IAssemblyLoader.LoadModule(string file)
		{
			lock (loaderLock)
			{
				IMetadataModule module = LoadDependencies(file);

				Debug.Assert(module != null);
				Debug.Assert(module.Metadata != null);

				return module;
			}
		}

		/// <summary>
		/// Gets the modules.
		/// </summary>
		/// <value>The modules.</value>
		IList<IMetadataModule> IAssemblyLoader.Modules { get { return modules.AsReadOnly(); } }

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		void IAssemblyLoader.AddPrivatePath(string path)
		{
			lock (loaderLock)
			{
				if (!privatePaths.Contains(path))
					privatePaths.Add(path);
			}
		}

		/// <summary>
		/// Initializes the private paths.
		/// </summary>
		/// <param name="assemblyPaths">The assembly paths.</param>
		void IAssemblyLoader.InitializePrivatePaths(IEnumerable<string> assemblyPaths)
		{
			foreach (string path in FindPrivatePaths(assemblyPaths))
				((IAssemblyLoader)this).AddPrivatePath(path);
		}

		#endregion // IAssemblyLoader

		#region Internals

		private bool isLoaded(string name)
		{
			foreach (var module in modules)
				if (module.Name == name)
					return true;

			return false;
		}

		private IMetadataModule LoadDependencies(string file)
		{
			var metadataModule = LoadAssembly(file);

			if (!isLoaded(metadataModule.Name))
			{
				var maxToken = metadataModule.Metadata.GetMaxTokenValue(TableType.AssemblyRef);

				foreach (var token in new Token(TableType.AssemblyRef, 1).Upto(maxToken))
				{
					var row = metadataModule.Metadata.ReadAssemblyRefRow(token);
					var assembly = metadataModule.Metadata.ReadString(row.Name);

					LoadDependencies(assembly);
				}

				if (!isLoaded(metadataModule.Name))
					modules.Add(metadataModule);
			}

			return metadataModule;
		}

		/// <summary>
		/// Loads the PE assembly.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		private IMetadataModule LoadPEAssembly(string file)
		{
			if (!File.Exists(file))
				return null;

			return new PortableExecutableImage(new FileStream(file, FileMode.Open, FileAccess.Read));
		}

		private IMetadataModule LoadAssembly(string file)
		{
			if (Path.IsPathRooted(file))
			{
				return LoadPEAssembly(file);
			}

			file = Path.GetFileName(file);

			if (!file.ToLower().EndsWith(".exe"))
				if (!file.ToLower().EndsWith(".dll"))
					file = file + ".dll";

			return TryAssemblyLoadFromPaths(file, privatePaths);
		}

		/// <summary>
		/// Does the load assembly From paths.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="paths">The paths.</param>
		/// <returns></returns>
		private IMetadataModule TryAssemblyLoadFromPaths(string name, IEnumerable<string> paths)
		{
			foreach (var path in paths)
			{
				try
				{
					var result = LoadPEAssembly(Path.Combine(path, name));

					if (result != null)
						return result;
				}
				catch
				{
					/* Failed to load assembly there... */
				}
			}

			return null;
		}

		/// <summary>
		/// Finds the private paths.
		/// </summary>
		/// <param name="assemblyPaths">The assembly paths.</param>
		/// <returns></returns>
		private IEnumerable<string> FindPrivatePaths(IEnumerable<string> assemblyPaths)
		{
			var privatePaths = new List<string>();

			foreach (var assembly in assemblyPaths)
			{
				var path = Path.GetDirectoryName(assembly);
				if (!privatePaths.Contains(path))
					privatePaths.Add(path);
			}

			return privatePaths;
		}

		#endregion // Internals

	}
}