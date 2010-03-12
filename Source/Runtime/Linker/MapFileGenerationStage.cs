/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.Linker
{
    /// <summary>
    /// An assembly compilation stage, which generates a map file of the built binary file.
    /// </summary>
	public sealed class MapFileGenerationStage : IAssemblyCompilerStage, IPipelineStage
    {
        #region Data members
		
		private IAssemblyLinker linker;

        /// <summary>
        /// Holds the text writer used to emit the map file.
        /// </summary>
        private TextWriter _writer;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFileGenerationStage"/> class.
        /// </summary>
        public MapFileGenerationStage(TextWriter writer)
        {
            if (null == writer)
                throw new ArgumentNullException(@"writer");

            _writer = writer;
        }

        #endregion // Construction

		#region IPipelineStage members

		string IPipelineStage.Name { get { return @"MapFileGenerationStage"; } }

		#endregion // IPipelineStage members

        #region IAssemblyCompilerStage Members
		
		public void Setup(AssemblyCompiler compiler)
		{
			this.linker = compiler.Pipeline.FindFirst<IAssemblyLinker>();
		}

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        public void Run()
        {
            // Emit map file _header
            _writer.WriteLine(linker.OutputFile);
            _writer.WriteLine();
            _writer.WriteLine("Timestamp is {0}", this.linker.TimeStamp);
            _writer.WriteLine();
            _writer.WriteLine("Preferred load address is {0:x16}", this.linker.BaseAddress);
            _writer.WriteLine();

            // Emit the sections
            EmitSections(this.linker);
            _writer.WriteLine();

            // Emit all symbols
            EmitSymbols(this.linker);
        }

        #endregion // IAssemblyCompilerStage Members

        #region Internals

        /// <summary>
        /// Emits all the section created in the binary file.
        /// </summary>
        /// <param name="linker">The assembly linker.</param>
        private void EmitSections(IAssemblyLinker linker)
        {
            _writer.WriteLine("Offset           Virtual          Length           Name                             Class");
            foreach (LinkerSection section in linker.Sections)
            {
                _writer.WriteLine("{0:x16} {1:x16} {2:x16} {3} {4}", section.Offset, section.VirtualAddress.ToInt64(), section.Length, section.Name.PadRight(32), section.SectionKind);
            }
        }

        /// <summary>
        /// Emits all symbols emitted in the binary file.
        /// </summary>
        /// <param name="linker">The assembly linker.</param>
        private void EmitSymbols(IAssemblyLinker linker)
        {
			_writer.WriteLine("Offset           Virtual          Length           Symbol");
            foreach (LinkerSymbol symbol in linker.Symbols)
				_writer.WriteLine("{0:x16} {1:x16} {2:x16} {3}", symbol.Offset, symbol.VirtualAddress.ToInt64(), symbol.Length, symbol.Name);

            LinkerSymbol entryPoint = linker.EntryPoint;
            if (entryPoint != null)
            {
                _writer.WriteLine();
                _writer.WriteLine("Entry point is {0}", entryPoint.Name);
                _writer.WriteLine("\tat offset {0:x16}", entryPoint.Offset);
                _writer.WriteLine("\tat virtual address {0:x16}", entryPoint.VirtualAddress.ToInt64());
            }
        }

        #endregion // Internals
    }
}
