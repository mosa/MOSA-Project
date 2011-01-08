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
using Mosa.Compiler.Linker;
using Mosa.Compiler.Linker.PE;
using Mosa.Compiler.FileFormat.PE;

namespace Mosa.Tools.Compiler
{
	/// <summary>
	///  Writes the CIL header into the generated binary.
	/// </summary>
	public sealed class CilHeaderBuilderStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, IPipelineStage
	{

		#region Data members

		private IAssemblyLinker linker;

		private CLI_HEADER cliHeader;

		#endregion // Data members

		#region IPipelineStage members

		string IPipelineStage.Name { get { return @"CILHeaderStage"; } }

		#endregion // IPipelineStage members

		#region IAssemblyCompilerStage Members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			linker = RetrieveAssemblyLinkerFromCompiler();
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IAssemblyCompilerStage.Run()
		{
			cliHeader.Cb = 0x48;
			cliHeader.MajorRuntimeVersion = 2;
			cliHeader.MinorRuntimeVersion = 0;
			cliHeader.Flags = RuntimeImageFlags.ILOnly;
			cliHeader.EntryPointToken = 0x06000001; // FIXME: ??

			LinkerSymbol metadata = this.linker.GetSymbol(Mosa.Runtime.Metadata.Symbol.Name);
			cliHeader.Metadata.VirtualAddress = (uint)(this.linker.GetSection(SectionKind.Text).VirtualAddress.ToInt64() + metadata.SectionAddress);
			cliHeader.Metadata.Size = (int)metadata.Length;

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
			using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII))
			{
				cliHeader.WriteTo(bw);
			}
		}

		#endregion // Internals

	}
}
