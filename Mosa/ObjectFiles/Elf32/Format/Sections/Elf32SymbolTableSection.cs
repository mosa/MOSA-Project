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
using System.Text;
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    /// <summary>
    /// 
    /// </summary>
    delegate void CreateSymbolHandler(Elf32Symbol symbol);

    /// <summary>
    /// This section holds a symbol table, as "Symbol Table'' in this chapter 
    /// describes. If a file has a loadable segment that includes the symbol table, 
    /// the section's attributes will include the SHF_ALLOC bit; otherwise, that bit 
    /// will be off.
    /// 
    /// Everything according to the specification in
    /// the TIS (Tool Interface Standard) ELF (Executable and Linking Format)
    /// Specification, 1-4, page 30.
    /// 
    /// Section's ELF Name: ".symtab"
    /// </summary>
    class Elf32SymbolTableSection : Elf32ProgbitsSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32SymbolTableSection"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public Elf32SymbolTableSection(Elf32File file)
            : base(file, ".symtab", Elf32SectionType.SHT_SYMTAB, Elf32SectionFlags.SHF_NONE)
        {
            this.Symbols = new Elf32SymbolCollection();
            Symbols.Add(null);
        }

        /// <summary>
        /// Gets or sets the symbols.
        /// </summary>
        /// <value>The symbols.</value>
        public Elf32SymbolCollection Symbols { get; private set; }

        /// <summary>
        /// Occurs when [on create symbol].
        /// </summary>
        public event CreateSymbolHandler OnCreateSymbol;

        /// <summary>
        /// Gets the symbol.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public Elf32Symbol GetSymbol(object data)
        {
            if (Symbols.Contains(data))
                return Symbols[data];
            else
            {
                Elf32Symbol sym = new Elf32Symbol(data);
                if (OnCreateSymbol != null)
                    OnCreateSymbol(sym);
                Symbols.Add(sym);
                return sym;
            }
        }

        /// <summary>
        /// This member holds a section header table index link, whose interpretation
        /// depends on the section type. A table below describes the values.
        /// </summary>
        /// <value></value>
        protected override int Link
        {
            get { return File.Sections.IndexOf(SymbolNames); }
        }

        /// <summary>
        /// Gets or sets the symbol names.
        /// </summary>
        /// <value>The symbol names.</value>
        public Elf32StringTableSection SymbolNames { get; set; }

        /// <summary>
        /// Some sections hold a table of fixed-size entries, such as a symbol table. For
        /// such a section, this member gives the size in bytes of each entry. The
        /// member contains 0 if the section does not hold a table of fixed-size entries.
        /// </summary>
        /// <value></value>
        public override int EntitySize { get { return Elf32Symbol.SYM_SIZE; } }

        /// <summary>
        /// Overloaded method to write the section's data accoring to the
        /// section's specification.
        /// Every section inherting Elf32ProgbitsSection has to implement
        /// this method.
        /// </summary>
        /// <param name="writer">Reference to the binary writer</param>
        protected override void WriteDataImpl(BinaryWriter writer)
        {
            Elf32File file = base.File;
            foreach (var sym in Symbols)
            {
                if (sym == null)
                    // Write the null symbol
                    writer.Write(new byte[Elf32Symbol.SYM_SIZE]);
                else
                    sym.Write(file, SymbolNames, writer);
            }
        }
    }
}
