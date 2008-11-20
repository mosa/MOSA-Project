/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman (<mailto:mail.alex.lyman@gmail.com>)
 */

using System;
using System.Collections.Generic;
using System.IO;

using Mosa.Runtime.Linker;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    /// <summary>
    /// 
    /// </summary>
    class Elf32RelocationSection : Elf32ProgbitsSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32RelocationSection"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="name">The name.</param>
        public Elf32RelocationSection(Elf32File file, string name)
            : base(file, name, Elf32SectionType.SHT_RELA, Elf32SectionFlags.SHF_NONE)
        {
        }

        /// <summary>
        /// This member holds a section header table index link, whose interpretation
        /// depends on the section type. A table below describes the values.
        /// </summary>
        /// <value></value>
        protected override int Link { get { return File.Sections.IndexOf(SymbolTable); } }

        /// <summary>
        /// This member holds extra information, whose interpretation depends on the
        /// section type. A table below describes the values.
        /// </summary>
        /// <value></value>
        protected override int Info { get { return File.Sections.IndexOf(TargetSection); } }

        /// <summary>
        /// Some sections hold a table of fixed-size entries, such as a symbol table. For
        /// such a section, this member gives the size in bytes of each entry. The
        /// member contains 0 if the section does not hold a table of fixed-size entries.
        /// </summary>
        /// <value></value>
        public override int EntitySize { get { return 12; } }

        /// <summary>
        /// Gets or sets the symbol table.
        /// </summary>
        /// <value>The symbol table.</value>
        public Elf32SymbolTableSection SymbolTable { get; set; }

        /// <summary>
        /// Gets or sets the target section.
        /// </summary>
        /// <value>The target section.</value>
        public Elf32Section TargetSection { get; set; }

        struct RelocA
        {
            /// <summary>
            /// 
            /// </summary>
            public int Offset;
            /// <summary>
            /// 
            /// </summary>
            public LinkType LinkType;
            /// <summary>
            /// 
            /// </summary>
            public Elf32Symbol Target;
            /// <summary>
            /// 
            /// </summary>
            public int Addend;
        }

        /// <summary>
        /// 
        /// </summary>
        List<RelocA> relocs = new List<RelocA>();

        /// <summary>
        /// Adds the specified link type.
        /// </summary>
        /// <param name="linkType">Type of the link.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="relative">The relative.</param>
        /// <param name="targetSym">The target sym.</param>
        public void Add(LinkType linkType, int offset, int relative, Elf32Symbol targetSym)
        {
            relocs.Add(
                new RelocA
                {
                    LinkType = linkType,
                    Offset = offset,
                    Addend = relative,
                    Target = targetSym
                }
            );
        }

        /// <summary>
        /// Writes the data impl.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void WriteDataImpl(BinaryWriter writer)
        {
            byte relative32Type, absolute32Type;
            switch (File.MachineKind)
            {
                case Elf32MachineKind.I386:
                    absolute32Type = 1;
                    relative32Type = 2;
                    break;
                default:
                    throw new NotSupportedException();
            }
            foreach (RelocA r in relocs)
            {
                writer.Write(r.Offset);
                int symIndex = SymbolTable.Symbols.IndexOf(r.Target);
                int type;
                switch (r.LinkType)
                {
                    case LinkType.I4 | LinkType.RelativeOffset:
                        type = relative32Type;
                        break;
                    case LinkType.I4 | LinkType.AbsoluteAddress:
                        type = absolute32Type;
                        break;
                    default: throw new NotSupportedException();
                }
                writer.Write((int)((symIndex << 8) | type));
                writer.Write(r.Addend);
            }
        }
    }
}
