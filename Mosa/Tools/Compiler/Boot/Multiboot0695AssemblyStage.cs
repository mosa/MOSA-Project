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
using Mosa.Runtime.CompilerFramework.Linker;
using System.IO;
using System.Diagnostics;

namespace Mosa.Tools.Compiler.Boot
{
    /*
     * FIXME:
     * - Allow video mode options to be controlled by the command line
     * - Allow the specification of additional load modules on the command line
     * - Write the multiboot compliant entry point, which parses the the boot
     *   information structure and populates the appropriate fields in the 
     *   KernelBoot entry.
     */

    /// <summary>
    /// Writes a multiboot v0.6.95 header into the generated binary.
    /// </summary>
    /// <remarks>
    /// This assembly compiler stage writes a multiboot header into the
    /// the data section of the binary file and also creates a multiboot
    /// compliant entry point into the binary.<para/>
    /// The header and entry point written by this stage is compliant with
    /// the specification at 
    /// http://www.gnu.org/software/grub/manual/multiboot/multiboot.html.
    /// </remarks>
    public sealed class Multiboot0695AssemblyStage : IAssemblyCompilerStage
    {
        #region Constants

        /// <summary>
        /// Magic value in the multiboot header.
        /// </summary>
        private const uint HEADER_MB_MAGIC = 0x1BADB002U;

        /// <summary>
        /// Multiboot flag, which indicates that additional modules must be
        /// loaded on page boundaries.
        /// </summary>
        private const uint HEADER_MB_FLAG_MODULES_PAGE_ALIGNED = 0x00000001U;

        /// <summary>
        /// Multiboot flag, which indicates if memory information must be
        /// provided in the boot information structure.
        /// </summary>
        private const uint HEADER_MB_FLAG_MEMORY_INFO_REQUIRED = 0x00000002U;

        /// <summary>
        /// Multiboot flag, which indicates that the supported video mode
        /// table must be provided in the boot information structure.
        /// </summary>
        private const uint HEADER_MB_FLAG_VIDEO_MODES_REQUIRED = 0x00000004U;

        /// <summary>
        /// Multiboot flag, which indicates a non-elf binary to boot and that
        /// settings for the executable file should be read from the boot header
        /// instead of the executable header.
        /// </summary>
        private const uint HEADER_MB_FLAG_NON_ELF_BINARY = 0x00010000U;

        #endregion // Constants

        #region Data members

        /// <summary>
        /// Holds the multiboot video mode.
        /// </summary>
        private uint videoMode;

        /// <summary>
        /// Holds the videoWidth of the screen for the video mode.
        /// </summary>
        private uint videoWidth;

        /// <summary>
        /// Holds the height of the screen for the video mode.
        /// </summary>
        private uint videoHeight;

        /// <summary>
        /// Holds the depth of the video mode in bits per pixel.
        /// </summary>
        private uint videoDepth;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Multiboot0695AssemblyStage"/> class.
        /// </summary>
        public Multiboot0695AssemblyStage()
        {
            videoMode = 1;
            videoWidth = 80;
            videoHeight = 25;
            videoDepth = 0;
        }

        #endregion // Construction

        #region IAssemblyCompilerStage Members

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public string Name
        {
            get { return @"MultibootAssemblyStage"; }
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public void Run(AssemblyCompiler compiler)
        {
            if (null != compiler)
                throw new ArgumentNullException(@"compiler");

            IAssemblyLinker linker = compiler.Pipeline.Find<IAssemblyLinker>();
            Debug.Assert(linker != null, @"No linker??");

            IntPtr entryPoint = WriteMultibootEntryPoint(linker);
            WriteMultibootHeader(linker, entryPoint);
        }

        #endregion // IAssemblyCompilerStage Members

        #region Internals

        /// <summary>
        /// Writes the multiboot entry point.
        /// </summary>
        /// <param name="linker">The linker.</param>
        /// <returns>The address of the real entry point.</returns>
        private IntPtr WriteMultibootEntryPoint(IAssemblyLinker linker)
        {
            /*
             * FIXME:
             * 
             * We can't use the standard entry point of the module. Instead
             * we write a multiboot compliant entry point here, which populates
             * the boot structure - so that it can be retrieved later.
             * 
             * Unfortunately this means, we need to define the boot structure
             * in the kernel to be able to access it later.
             * 
             */

            return IntPtr.Zero;
        }

        /// <summary>
        /// Writes the multiboot header.
        /// </summary>
        /// <param name="linker">The linker.</param>
        /// <param name="entryPoint">The address of the multiboot compliant entry point.</param>
        private void WriteMultibootHeader(IAssemblyLinker linker, IntPtr entryPoint)
        {
            using (Stream stream = linker.Allocate(@"boot", SectionKind.ROData, 64, 4))
            using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII))
            {
                // FIXME: Extract multiboot information from the linker and store it
                // in the multiboot format. Write it directly into the stream
                uint flags = HEADER_MB_FLAG_VIDEO_MODES_REQUIRED | HEADER_MB_FLAG_MEMORY_INFO_REQUIRED | HEADER_MB_FLAG_MODULES_PAGE_ALIGNED;
                //if (!(linker is ELFLinker))
                //    flags |= HEADER_MB_FLAG_NON_ELF_BINARY;

                uint csum = unchecked(0U - HEADER_MB_MAGIC - flags);
                bw.Write(HEADER_MB_MAGIC);
                bw.Write(flags);
                bw.Write(csum);

                if (HEADER_MB_FLAG_NON_ELF_BINARY == (flags & HEADER_MB_FLAG_NON_ELF_BINARY))
                {
                    // FIXME: Write non-standard header info
                    // linker.BaseAddress and LinkerSection.Address...
                }
                else
                {
                    // Fill the executable header fields in the boot information with zero
                    uint zero = 0;
                    bw.Write(zero); // header_addr
                    bw.Write(zero); // load_addr
                    bw.Write(zero); // load_end_addr
                    bw.Write(zero); // bss_end_addr
                }

                bw.Write((uint)entryPoint.ToInt32());

                // Write the video mode
                bw.Write(videoMode);
                bw.Write(videoWidth);
                bw.Write(videoHeight);
                bw.Write(videoDepth);
            }
        }

        #endregion // Internals
    }
}
