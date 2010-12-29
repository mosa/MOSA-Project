/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

using Mosa.Runtime.Metadata.Loader.PE;

namespace Mosa.Runtime.Metadata.Loader
{
	/// <summary>
	/// Provides a default implementation of the IAssemblyLoader interface.
	/// </summary>
	public class AssemblyLoader : IAssemblyLoader
	{
		#region Data members

		private string[] searchPath;
		private List<string> privatePaths = new List<string>();
		private object loaderLock = new object();

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyLoader"/> class.
		/// </summary>
		public AssemblyLoader()
		{
			// HACK: I can't figure out an easier way to get the framework dir right now...
			//string frameworkDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
			//string frameworkDir32 = frameworkDir.Contains("Framework64") ? frameworkDir.Replace("Framework64", "Framework") : frameworkDir;
			searchPath = new[] {
				AppDomain.CurrentDomain.BaseDirectory,
				//frameworkDir,
				//frameworkDir32
			};
		}

		#endregion // Construction

		#region IAssemblyLoader

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		void IAssemblyLoader.AppendPrivatePath(string path)
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
				((IAssemblyLoader)this).AppendPrivatePath(path);
		}

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
				IMetadataModule module = LoadAssembly(file);

				Debug.Assert(module != null);
				Debug.Assert(module.Metadata != null);

				return module;
			}
		}

		#endregion // IAssemblyLoader

		#region Internals

		public static string CreateFileCodeBase(string file)
		{
			return @"file://" + file.Replace('\\', '/');
		}

		private IMetadataModule LoadAssembly(string file)
		{

			if (Path.IsPathRooted(file))
			{
				return LoadAssembly2(file); 
			}

			file = Path.GetFileName(file);

			if (!file.EndsWith(".dll"))
				file = file + ".dll";

			IMetadataModule result = DoLoadAssembly(file);

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

			if (!File.Exists(file))
				return null;

			return PortableExecutableImage.Load(new FileStream(file, FileMode.Open, FileAccess.Read), codeBase);
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
			foreach (string path in paths)
			{
				string fullName = Path.Combine(path, name);
				try
				{
					IMetadataModule result = LoadAssembly(fullName);
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
			List<string> privatePaths = new List<string>();

			foreach (string assembly in assemblyPaths)
			{
				string path = Path.GetDirectoryName(assembly);
				if (!privatePaths.Contains(path))
					privatePaths.Add(path);
			}

			return privatePaths;
		}

		#endregion // Internals

	}
}