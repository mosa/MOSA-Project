using Mosa.Compiler.Metadata.Loader;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaAssemblyLoader
	{
		public IList<IMetadataModule> Modules { get; private set; }

		private List<string> privatePaths = new List<string>();

		public MosaAssemblyLoader()
		{
			Modules = new List<IMetadataModule>();
		}

		#region Internal methods

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

		private bool IsLoaded(string name)
		{
			foreach (var module in Modules)
				if (module.Name == name)
					return true;

			return false;
		}

		private IMetadataModule LoadDependencies(string file)
		{
			var metadataModule = LoadAssembly(file);

			if (!IsLoaded(metadataModule.Name))
			{
				var maxToken = metadataModule.Metadata.GetMaxTokenValue(TableType.AssemblyRef);

				foreach (var token in new Token(TableType.AssemblyRef, 1).Upto(maxToken))
				{
					var row = metadataModule.Metadata.ReadAssemblyRefRow(token);
					var assembly = metadataModule.Metadata.ReadString(row.Name);

					LoadDependencies(assembly);
				}

				if (!IsLoaded(metadataModule.Name))
					Modules.Add(metadataModule);
			}

			return metadataModule;
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
				var result = LoadPEAssembly(Path.Combine(path, name));

				if (result != null)
					return result;
			}

			return null;
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

		#endregion

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>
		public void AddPrivatePath(string path)
		{
			if (!privatePaths.Contains(path))
			{
				privatePaths.Add(path);
			}
		}

		/// <summary>
		/// Appends the given paths to the assembly search path.
		/// </summary>
		/// <param name="assemblyPaths">The assembly paths.</param>
		public void InitializePrivatePaths(IEnumerable<string> assemblyPaths)
		{
			foreach (var path in FindPrivatePaths(assemblyPaths))
			{
				AddPrivatePath(path);
			}
		}

		/// <summary>
		/// Loads the named assembly.
		/// </summary>
		/// <param name="file">The file path of the assembly to load.</param>
		/// <returns>
		/// The assembly image of the loaded assembly.
		/// </returns>
		public void LoadModule(string file)
		{
			var module = LoadDependencies(file);

			Debug.Assert(module != null);
			Debug.Assert(module.Metadata != null);
		}

	}
}