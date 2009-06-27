/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
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
    public sealed class MapFileGenerationStage : IAssemblyCompilerStage
    {
        #region Data members

        /// <summary>
        /// Holds the text writer used to emit the map file.
        /// </summary>
        private TextWriter writer;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFileGenerationStage"/> class.
        /// </summary>
        public MapFileGenerationStage(TextWriter writer)
        {
            if (null == writer)
                throw new ArgumentNullException(@"writer");

            this.writer = writer;
        }

        #endregion // Construction

        #region IAssemblyCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return @"MapFileGenerationStage"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(AssemblyCompiler compiler)
        {
            // Retrieve the linker
            IAssemblyLinker linker = compiler.Pipeline.Find<IAssemblyLinker>();

            // Emit map file header
            this.writer.WriteLine(linker.OutputFile);
            this.writer.WriteLine();
            this.writer.WriteLine("Timestamp is {0}", linker.TimeStamp);
            this.writer.WriteLine();
            this.writer.WriteLine("Preferred load address is {0:x16}", linker.BaseAddress);
            this.writer.WriteLine();

            // Emit the sections
            EmitSections(linker);
            this.writer.WriteLine();

            // Emit all symbols
            EmitSymbols(linker);
        }

        #endregion // IAssemblyCompilerStage Members

        #region Internals

        /// <summary>
        /// Emits all the section created in the binary file.
        /// </summary>
        /// <param name="linker">The assembly linker.</param>
        private void EmitSections(IAssemblyLinker linker)
        {
            this.writer.WriteLine("Offset           Virtual          Length           Name                             Class");
            foreach (LinkerSection section in linker.Sections)
            {
                this.writer.WriteLine("{0:x16} {1:x16} {2:x16} {3} {4}", section.Offset, section.VirtualAddress.ToInt64(), section.Length, section.Name.PadRight(32), section.SectionKind);
            }
        }

        /// <summary>
        /// Emits all symbols emitted in the binary file.
        /// </summary>
        /// <param name="linker">The assembly linker.</param>
        private void EmitSymbols(IAssemblyLinker linker)
        {
			this.writer.WriteLine("Offset           Virtual          Length           Symbol");
            foreach (LinkerSymbol symbol in linker.Symbols)
            {
				this.writer.WriteLine("{0:x16} {1:x16} {2:x16} {3}", symbol.Offset, symbol.VirtualAddress.ToInt64(), symbol.Length, symbol.Name);
            }

            LinkerSymbol entryPoint = linker.EntryPoint;
            if (null != entryPoint)
            {
                this.writer.WriteLine();
                this.writer.WriteLine("Entry point is {0}", entryPoint.Name);
                this.writer.WriteLine("\tat offset {0:x16}", entryPoint.Offset);
                this.writer.WriteLine("\tat virtual address {0:x16}", entryPoint.VirtualAddress.ToInt64());
            }
        }

        #endregion // Internals
    }
}
