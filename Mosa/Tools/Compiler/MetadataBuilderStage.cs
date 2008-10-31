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
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using System.IO;
using Mosa.Runtime.Loader;

namespace Mosa.Tools.Compiler
{
    /// <summary>
    /// This stage adds the CIL metadata and MOSA AOT metadata to the compiled assembly.
    /// </summary>
    sealed class MetadataBuilderStage : IAssemblyCompilerStage
    {
        #region Data Members

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataBuilderStage"/> class.
        /// </summary>
        public MetadataBuilderStage()
        {
        }

        #endregion // Construction

        #region IAssemblyCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"Metadata Builder Stage"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(AssemblyCompiler compiler)
        {
            IAssemblyLinker linker = compiler.Pipeline.Find<IAssemblyLinker>();
            if (linker == null)
                throw new InvalidOperationException(@"Can't run without a linker.");

            // FIXME: Retrieve the compilation target assembly
            IMetadataModule module = null;

            /*
             * Metadata is in the .text section in order to make it relocatable everywhere.
             * 
             */
            using (Stream metadata = linker.Allocate(@".cil.metadata", SectionKind.Text, 0, 0))
            {
                ExportCilMetadata(metadata, module);
            }
        }

        #endregion // IAssemblyCompilerStage Members

        #region Internals

        /// <summary>
        /// Exports the CIL metadata.
        /// </summary>
        /// <param name="metadata">The metadata stream to write too.</param>
        /// <param name="module">The module.</param>
        private void ExportCilMetadata(Stream metadata, IMetadataModule module)
        {
            /*
             * FIXME:
             * - Obtain CIL & MOSA metadata tables
             * - Write metadata root
             * - Write the CIL tables (modified)
             * - Write the MOSA tables (new)
             * - Write the strings, guid and blob heap (unchanged from original module)
             */
        }

        #endregion // Internals
    }
}
