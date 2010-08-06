/*
 * (c) 2010 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Vm;
using Mosa.Tools.Compiler.Symbols.Pdb;
using Mosa.Tools.Compiler.TypeInitializers;
using Mosa.Runtime.Loader.PE;

namespace Mosa.Tools.Compiler.Metadata
{

	/// <summary>
	/// The MergedMetadata class consolidates multiple IMetadataModule provides into a single IMetadataModule view. 
	/// </summary>
	public class MergedAssemblyLoader : IAssemblyLoader
	{
		protected MergedMetadata mergedMetadata;
		protected List<IMetadataModule> loadedImages = new List<IMetadataModule>();

		public IMetadataModule MetadataModule { get { return mergedMetadata; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="MergedMetadata"/> class.
		/// </summary>
		/// <param name="modules">The modules.</param>
		public MergedAssemblyLoader(IEnumerable<string> inputFileNames, ITypeSystem typeSystem)
		{

			foreach (string file in inputFileNames)
			{
				IMetadataModule assembly;
				string codeBase = CreateFileCodeBase(file);

				if (File.Exists(file))
				{
					assembly = PortableExecutableImage.Load(loadedImages.Count, new FileStream(file, FileMode.Open, FileAccess.Read), codeBase);
					if (assembly != null)
					{
						loadedImages.Add(assembly);
					}
				}

			}

			this.mergedMetadata = new MergedMetadata(loadedImages);
			typeSystem.AssemblyLoaded(this.mergedMetadata);
		}

		private static IMetadataModule LoadAssembly(IAssemblyLoader assemblyLoader, string assemblyFileName)
		{
			try
			{
				IMetadataModule assemblyModule = assemblyLoader.Load(assemblyFileName);

				// Try to load debug information for the compilation
				LoadAssemblyDebugInfo(assemblyFileName);

				return assemblyModule;
			}
			catch (BadImageFormatException bife)
			{
				throw new CompilationException(String.Format("Couldn't load input file {0} (invalid format).", assemblyFileName), bife);
			}
		}

		/// <summary>
		/// Loads the assembly.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		private IMetadataModule LoadAssembly(string file)
		{
			return (IMetadataModule)this;
		}

		private static string CreateFileCodeBase(string file)
		{
			return @"file://" + file.Replace('\\', '/');
		}

		private static void LoadAssemblyDebugInfo(string assemblyFileName)
		{
			string dbgFile = Path.Combine(Path.GetDirectoryName(assemblyFileName), Path.GetFileNameWithoutExtension(assemblyFileName) + ".pdb") + "!!";

			if (File.Exists(dbgFile))
			{
				using (FileStream fileStream = new FileStream(dbgFile, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (PdbReader reader = new PdbReader(fileStream))
					{
						Debug.WriteLine(@"Global symbols:");
						foreach (CvSymbol symbol in reader.GlobalSymbols)
						{
							Debug.WriteLine("\t" + symbol.ToString());
						}

						Debug.WriteLine(@"Types:");
						foreach (PdbType type in reader.Types)
						{
							Debug.WriteLine("\t" + type.Name);
							Debug.WriteLine("\t\tSymbols:");
							foreach (CvSymbol symbol in type.Symbols)
							{
								Debug.WriteLine("\t\t\t" + symbol.ToString());
							}

							Debug.WriteLine("\t\tLines:");
							foreach (CvLine line in type.LineNumbers)
							{
								Debug.WriteLine("\t\t\t" + line.ToString());
							}
						}
					}
				}
			}
		}

		#region IAssemblyLoader members

		/// <summary>
		/// Appends the given path to the assembly search path.
		/// </summary>
		/// <param name="path">The path to append to the assembly search path.</param>

		void IAssemblyLoader.AppendPrivatePath(string path)
		{
			return;
		}

		/// <summary>
		/// Resolves the given assembly reference and loads the associated IMetadataModule.
		/// </summary>
		/// <param name="provider">The metadata provider, which contained the assembly reference.</param>
		/// <param name="assemblyRef">The assembly reference to resolve.</param>
		/// <returns>An instance of IMetadataModule representing the resolved assembly.</returns>
		IMetadataModule IAssemblyLoader.Resolve(IMetadataProvider provider, AssemblyRefRow assemblyRef)
		{
			return mergedMetadata;
		}

		/// <summary>
		/// Loads the named assembly.
		/// </summary>
		/// <param name="file">The file path of the assembly to load.</param>
		/// <returns>The assembly image of the loaded assembly.</returns>
		IMetadataModule IAssemblyLoader.Load(string file)
		{
			return mergedMetadata;
		}

		/// <summary>
		/// Unloads the given module.
		/// </summary>
		/// <param name="module">The module to unload.</param>
		void IAssemblyLoader.Unload(IMetadataModule module)
		{
			return;
		}

		/// <summary>
		/// Gets an enumerable collection of loaded modules.
		/// </summary>
		IEnumerable<IMetadataModule> IAssemblyLoader.Modules
		{
			get
			{
				yield return mergedMetadata;
			}
		}

		/// <summary>
		/// Gets the module.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		IMetadataModule IAssemblyLoader.GetModule(int index)
		{
			return mergedMetadata;
		}

		#endregion // IAssemblyLoader members

	}
}
