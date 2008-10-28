using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.Runtime.CompilerFramework.Linker
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
            this.writer.WriteLine(compiler.Assembly.Name);
            this.writer.WriteLine();
            this.writer.WriteLine("Timestamp is {0} ({1})", linker.TimeStamp, linker.TimeStamp);
            this.writer.WriteLine();
            this.writer.WriteLine("Preferred load address is {0}", linker.BaseAddress);
            this.writer.WriteLine();

            // Emit the sections
            EmitSections(linker);

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
            this.writer.WriteLine("Start            Length           Name                            Class");
            foreach (LinkerSection section in linker.Sections)
            {
                this.writer.WriteLine("{0:X16} {1:x16} {2:s32} {3}", section.Address, section.Length, section.Name, section.SectionKind);
            }
        }

        /// <summary>
        /// Emits all symbols emitted in the binary file.
        /// </summary>
        /// <param name="linker">The assembly linker.</param>
        private void EmitSymbols(IAssemblyLinker linker)
        {
            this.writer.WriteLine("Address              RVA               Symbol");
            foreach (LinkerSymbol symbol in linker.Symbols)
            {
                this.writer.WriteLine("{0:x16} {1:x16} {2}", symbol.Address, symbol.Address, symbol.Name);
            }

            LinkerSymbol entryPoint = linker.EntryPoint;
            if (null != entryPoint)
            {
                this.writer.WriteLine("Entry point is at {0} ({1})", entryPoint.Address, entryPoint.Name);
            }
        }

        #endregion // Internals
    }
}
