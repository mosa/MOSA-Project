/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Kai P. Reisert <kpreisert@googlemail.com>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Linker.PE;

namespace Mosa.Tools.Compiler
{
	/// <summary>
	///  Writes the cil _header into the generated binary.
	/// </summary>
	public sealed class CilHeaderBuilderStage : IAssemblyCompilerStage, IPipelineStage
	{

		#region Data members
		
		private AssemblyCompiler compiler;
		
		private IAssemblyLinker linker;

		private CLI_HEADER _cliHeader;

		#endregion // Data members		

		#region IPipelineStage members

		string IPipelineStage.Name { get { return @"CILHeaderStage"; } }

		#endregion // IPipelineStage members

		#region IAssemblyCompilerStage Members
		
		public void Setup(AssemblyCompiler compiler)
		{
			this.compiler = compiler;
			this.linker = compiler.Pipeline.FindFirst<IAssemblyLinker>();
			Debug.Assert(linker != null, @"No linker??");
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			_cliHeader.Cb = 0x48;
			_cliHeader.MajorRuntimeVersion = 2;
			_cliHeader.MinorRuntimeVersion = 0;
			_cliHeader.Flags = RuntimeImageFlags.ILOnly;
			_cliHeader.EntryPointToken = 0x06000001; // FIXME: ??

			LinkerSymbol metadata = this.linker.GetSymbol(Mosa.Runtime.Metadata.Symbol.Name);
			_cliHeader.Metadata.VirtualAddress = (uint)(this.linker.GetSection(SectionKind.Text).VirtualAddress.ToInt64() + metadata.SectionAddress);
			_cliHeader.Metadata.Size = (int)metadata.Length;

			WriteCilHeader();
		}

		#endregion // IAssemblyCompilerStage Members

		#region Internals

		/// <summary>
		/// Writes the Cil _header.
		/// </summary>
		private void WriteCilHeader()
		{
			using (Stream stream = this.linker.Allocate(CLI_HEADER.SymbolName, SectionKind.Text, CLI_HEADER.Length, 4))
			using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII)) {
				_cliHeader.Write(bw);
			}
		}

		#endregion // Internals

	}
}
