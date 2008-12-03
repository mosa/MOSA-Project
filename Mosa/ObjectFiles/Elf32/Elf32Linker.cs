using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Linker;

namespace Mosa.ObjectFiles.Elf32
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32Linker : Mosa.Runtime.Linker.AssemblyLinkerStageBase
    {
        /// <summary>
        /// Holds the name of the output file.
        /// </summary>
        private string outputFile = null;

        /// <summary>
        /// 
        /// </summary>
        private List<Mosa.Runtime.Linker.LinkerSection> sections;

        /// <summary>
        /// Retrieves the collection of sections created during compilation.
        /// </summary>
        /// <value>The sections collection.</value>
        public override ICollection<Mosa.Runtime.Linker.LinkerSection> Sections
        {
            get
            {
                return this.sections;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32Linker"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public Elf32Linker(string file)
        {
            if (String.IsNullOrEmpty(file) == true)
                throw new ArgumentException(@"Invalid argument.", @"outputFile");

            this.outputFile = file;
            this.sections = new List<LinkerSection>();

            // Create the default section set
            Elf32Section[] sections = new Elf32Section[(int)SectionKind.Max];
            sections[(int)SectionKind.Text]   = new Sections.Elf32CodeSection();
            sections[(int)SectionKind.Data]   = new Sections.Elf32DataSection();
            sections[(int)SectionKind.ROData] = new Sections.Elf32RoDataSection();
            sections[(int)SectionKind.BSS]    = new Sections.Elf32BssSection();
            this.sections.AddRange(sections);
        }

        /// <summary>
        /// Performs stage specific processing on the compiler context.
        /// </summary>
        /// <param name="compiler">The compiler context to perform processing in.</param>
        public override void Run(Mosa.Runtime.CompilerFramework.AssemblyCompiler compiler)
        {
            base.Run(compiler);
        }

        /// <summary>
        /// Retrieves the name of the compilation stage.
        /// </summary>
        /// <value>The name of the compilation stage.</value>
        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Special resolution for internal calls.
        /// </summary>
        /// <param name="method">The internal call method to resolve.</param>
        /// <returns>The address</returns>
        protected override long ResolveInternalCall(Mosa.Runtime.Vm.RuntimeMethod method)
        {
            return base.ResolveInternalCall(method);
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
        protected override System.IO.Stream Allocate(Mosa.Runtime.Linker.SectionKind section, int size, int alignment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A request to patch already emitted code by storing the calculated address value.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="methodAddress">The virtual address of the method whose code is being patched.</param>
        /// <param name="methodOffset">The value to store at the position in code.</param>
        /// <param name="methodRelativeBase">The method relative base.</param>
        /// <param name="targetAddress">The position in code, where it should be patched.</param>
        protected override void ApplyPatch(Mosa.Runtime.Linker.LinkType linkType, long methodAddress, long methodOffset, long methodRelativeBase, long targetAddress)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a linker section by its type.
        /// </summary>
        /// <param name="sectionKind">The type of the section to retrieve.</param>
        /// <returns>The retrieved linker section.</returns>
        protected override Mosa.Runtime.Linker.LinkerSection GetSection(Mosa.Runtime.Linker.SectionKind sectionKind)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the specified symbol is resolved.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="address">The address.</param>
        /// <returns>
        /// 	<c>true</c> if the specified symbol is resolved; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsResolved(string symbol, out long address)
        {
            return base.IsResolved(symbol, out address);
        }

        /// <summary>
        /// Creates the elf32 file.
        /// </summary>
        private void CreateElf32File()
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(this.outputFile, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {

            }
        }
    }
}
