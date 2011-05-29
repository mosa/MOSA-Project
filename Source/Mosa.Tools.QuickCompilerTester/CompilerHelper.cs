/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fr鰄lich (grover) <michael.ruck@michaelruck.de>
 */

using System;
using System.Collections.Generic;

using Mosa.Runtime.CompilerFramework;
using Mosa.Compiler.Linker;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.TypeSystem;
using System.IO;
using Mosa.Runtime.Linker;
//using Mosa.Runtime.InternalLog;

using x86 = Mosa.Platform.x86;

namespace Mosa.Tools.CompilerHelper
{
	public delegate void CCtor();
	class LinkerStub : BaseAssemblyLinkerStage, IAssemblyLinker, IPipelineStage
	{
		string IPipelineStage.Name { get { return @"Stub Linker"; } }
		
		public override long LoadSectionAlignment
		{
			get { return 1; }
		}

		/// <summary>
		/// Gets the virtual alignment of sections.
		/// </summary>
		/// <value>The virtual section alignment.</value>
		public override long VirtualSectionAlignment
		{
			get { return 1; }
		}

		/// <summary>
		/// Retrieves a linker section by its type.
		/// </summary>
		/// <param name="sectionKind">The type of the section to retrieve.</param>
		/// <returns>The retrieved linker section.</returns>
		public override LinkerSection GetSection(SectionKind sectionKind)
		{
			return null;
		}

		/// <summary>
		/// Retrieves the collection of sections created during compilation.
		/// </summary>
		/// <value>The sections collection.</value>
		public override ICollection<LinkerSection> Sections
		{
			get { return null; }
		}

		/// <summary>
		/// Allocates a symbol of the given name in the specified section.
		/// </summary>
		/// <param name="section">The executable section to allocate from.</param>
		/// <param name="size">The number of bytes to allocate. If zero, indicates an unknown amount of memory is required.</param>
		/// <param name="alignment">The alignment. A value of zero indicates the use of a default alignment for the section.</param>
		/// <returns>
		/// A stream, which can be used to populate the section.
		/// </returns>
		protected override Stream Allocate(SectionKind section, int size, int alignment)
		{
			return new MemoryStream();
		}
		public override Stream Allocate(string name, SectionKind section, int size, int alignment)
		{
			return new MemoryStream();
		}
		protected override void ApplyPatch(LinkType linkType, long methodAddress, long methodOffset, long methodRelativeBase, long targetAddress)
		{ }
		public override void Run()
		{ }

	}

	class CompilerHelper : AssemblyCompiler
	{
		//private readonly Queue<CCtor> cctorQueue = new Queue<CCtor>();

		//private readonly TestAssemblyLinker linker;

		private CompilerHelper(IArchitecture architecture, ITypeSystem typeSystem, ITypeLayout typeLayout) :
			base(architecture, typeSystem, typeLayout, null)
		{
			var linker = new LinkerStub();

			// Build the assembly compiler pipeline
			Pipeline.AddRange(new IAssemblyCompilerStage[] {
				new AssemblyMemberCompilationSchedulerStage(),
				new MethodCompilerSchedulerStage(),
				new TypeLayoutStage(),
				linker
			});
			architecture.ExtendAssemblyCompilerPipeline(Pipeline);
		}

		public static void Compile(ITypeSystem typeSystem)
		{
			IArchitecture architecture = x86.Architecture.CreateArchitecture(x86.ArchitectureFeatureFlags.AutoDetect);

			// FIXME: get from architecture
			TypeLayout typeLayout = new TypeLayout(typeSystem, 4, 4);

			CompilerHelper compiler = new CompilerHelper(architecture, typeSystem, typeLayout);
			compiler.Compile();

			//return compiler.linker;
		}

		public static void FilterMethod(string s)
		{
			//InstructionLogger.classfilter = s;
		}

		public static void SetLogger(ILoggerWriter ilw)
		{
			//InstructionLogger.loggerWriter = ilw;
			//InstructionLogger.output = true;
		}

		public override IMethodCompiler CreateMethodCompiler(ICompilationSchedulerStage schedulerStage, RuntimeType type, RuntimeMethod method)
		{
			IMethodCompiler mc = new MethodCompiler(this, Architecture, schedulerStage, type, method);
			Architecture.ExtendMethodCompilerPipeline(mc.Pipeline);
			return mc;
		}

		protected override void EndCompile()
		{
			base.EndCompile();

			/*while (this.cctorQueue.Count > 0)
			{
				CCtor cctor = this.cctorQueue.Dequeue();
				cctor();
			}*/
		}

		public void QueueCCtorForInvocationAfterCompilation(CCtor cctor)
		{
			//cctorQueue.Enqueue(cctor);
		}

	}
}
