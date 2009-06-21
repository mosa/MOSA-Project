/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Kai P. Reisert (<mailto:kpreisert@googlemail.com>)
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;

namespace Mosa.Tools.Compiler
{

	/// <summary>
	///  Writes the cil header into the generated binary.
	/// </summary>
	public sealed class CilHeaderBuilderStage : IAssemblyCompilerStage
	{
		#region Constants

		#endregion // Constants

		#region Data members

		#endregion // Data members

		#region Construction

		#endregion // Construction

		#region IAssemblyCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"CILHeaderStage"; }
		}

		private const string CILHeaderSymbolName = @".cil.header";

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(AssemblyCompiler compiler)
		{
			if (compiler == null)
				throw new ArgumentNullException(@"compiler");

			IAssemblyLinker linker = compiler.Pipeline.Find<IAssemblyLinker>();
			Debug.Assert(linker != null, @"No linker??");

			WriteCilHeader(compiler, linker);
		}

		#endregion // IAssemblyCompilerStage Members

		#region Internals

		/// <summary>
		/// Writes the Cil header.
		/// </summary>
		/// <param name="compiler">The assembly compiler.</param>
		/// <param name="linker">The linker.</param>
		private void WriteCilHeader(AssemblyCompiler compiler, IAssemblyLinker linker)
		{
			int length = 0x250 - 0x208;
			using (Stream stream = linker.Allocate(CILHeaderSymbolName, SectionKind.Text, length, 4))
			using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII)) {
				// TODO: Output CIL Header

				// HACK
				for (int i = 0; i < length; i++)
					bw.Write((byte)0);
			}
		}

		#endregion // Internals

	}
}
