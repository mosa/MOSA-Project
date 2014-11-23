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

		public CompilationScheduler CompilationScheduler { get; private set; }

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
			{
				yield return file.FullName;
			}
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

			CompilationScheduler = new CompilationScheduler(TypeSystem);
		}

		public void Execute()
		{
			Initialize();
			PreCompile();
			ScheduleAll();
			Compile();
			PostCompile();
		}

		public void Initialize()
		{
			Linker = CompilerOptions.LinkerFactory();
			Linker.Initialize(CompilerOptions.BaseAddress, CompilerOptions.Architecture.Endianness, CompilerOptions.Architecture.ElfMachineType);

			BaseCompiler = CompilerEngineFactory();

			BaseCompiler.Initialize(this);
		}

		private void PreCompile()
		{
			BaseCompiler.PreCompile();
		}

		private void ScheduleAll()
		{
			CompilationScheduler.ScheduleAll();
		}

		private void Schedule(MosaType type)
		{
			CompilationScheduler.Schedule(type);
		}

		private void Schedule(MosaMethod method)
		{
			CompilationScheduler.Schedule(method);
		}

		private void Compile()
		{
			BaseCompiler.Compile();
		}

		private void PostCompile()
		{
			BaseCompiler.PostCompile();
		}

	}
}