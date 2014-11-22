/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Compiler.Framework
{
	public class MosaCompiler
	{
		public Func<BaseCompiler> CompilerEngineFactory { get; set; }

		protected BaseCompiler BaseCompiler { get; private set; }

		public CompilerOptions CompilerOptions { get; set; }

		public CompilerTrace CompilerTrace { get; private set; }

		protected MosaModuleLoader ModuleLoader { get; private set; }

		public TypeSystem TypeSystem { get; private set; }

		public MosaTypeLayout TypeLayout { get; private set; }

		public ICompilationScheduler CompilationScheduler { get; private set; }

		public BaseLinker Linker { get; private set; }

		public MosaCompiler()
		{
			CompilerOptions = new CompilerOptions();
			CompilerTrace = new CompilerTrace();
			ModuleLoader = new MosaModuleLoader();
		}

		/// <summary>
		/// Gets a list of input file names.
		/// </summary>
		private static IEnumerable<string> GetInputFileNames(List<FileInfo> inputFiles)
		{
			foreach (FileInfo file in inputFiles)
				yield return file.FullName;
		}

		public void Load(List<FileInfo> inputFiles)
		{
			ModuleLoader.AddPrivatePath(GetInputFileNames(inputFiles));

			foreach (string file in GetInputFileNames(inputFiles))
			{
				ModuleLoader.LoadModuleFromFile(file);
			}

			Load(TypeSystem.Load(ModuleLoader.CreateMetadata()));
		}

		public void Load(TypeSystem typeSystem)
		{
			TypeSystem = typeSystem;

			TypeLayout = new MosaTypeLayout(typeSystem, CompilerOptions.Architecture.NativePointerSize, CompilerOptions.Architecture.NativeAlignment);

			CompilationScheduler = new CompilationScheduler(TypeSystem, true);
		}

		public void StartCompiler()
		{
			Linker = CompilerOptions.LinkerFactory();
			Linker.Initialize(CompilerOptions.BaseAddress, CompilerOptions.Architecture.Endianness, CompilerOptions.Architecture.ElfMachineType);

			BaseCompiler = CompilerEngineFactory();

			BaseCompiler.Initialize(this);

			BaseCompiler.Compile();
		}

		private void CompilerType(MosaType type)
		{ }

		private void CompilerMethod(MosaMethod method)
		{ }

		private void ResolveSymbols()
		{ }

		private void FinalizeOutput()
		{ }
	}
}