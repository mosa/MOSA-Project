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

		private CLI_HEADER _cliHeader;

		#endregion // Data members		

		#region IPipelineStage members

		string IPipelineStage.Name { get { return @"CILHeaderStage"; } }

		#endregion // IPipelineStage members

		#region IAssemblyCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(AssemblyCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			IAssemblyLinker linker = compiler.Pipeline.FindFirst<IAssemblyLinker>();
			Debug.Assert(linker != null, @"No linker??");

			_cliHeader.Cb = 0x48;
			_cliHeader.MajorRuntimeVersion = 2;
			_cliHeader.MinorRuntimeVersion = 0;
			_cliHeader.Flags = RuntimeImageFlags.ILOnly;
			_cliHeader.EntryPointToken = 0x06000001; // FIXME: ??

			LinkerSymbol metadata = linker.GetSymbol(Mosa.Runtime.Metadata.Symbol.Name);
			_cliHeader.Metadata.VirtualAddress = (uint)(linker.GetSection(SectionKind.Text).VirtualAddress.ToInt64() + metadata.SectionAddress);
			_cliHeader.Metadata.Size = (int)metadata.Length;

			WriteCilHeader(compiler, linker);
		}

		#endregion // IAssemblyCompilerStage Members

		#region Internals

		/// <summary>
		/// Writes the Cil _header.
		/// </summary>
		/// <param name="compiler">The assembly compiler.</param>
		/// <param name="linker">The linker.</param>
		private void WriteCilHeader(AssemblyCompiler compiler, IAssemblyLinker linker)
		{
			using (Stream stream = linker.Allocate(CLI_HEADER.SymbolName, SectionKind.Text, CLI_HEADER.Length, 4))
			using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII)) {
				_cliHeader.Write(bw);
			}
		}

		#endregion // Internals

	}
}
