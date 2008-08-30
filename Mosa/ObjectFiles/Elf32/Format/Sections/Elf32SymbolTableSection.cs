using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mosa.ObjectFiles.Elf32.Format.Sections
{
    delegate void CreateSymbolHandler(Elf32Symbol symbol);

    class Elf32SymbolTableSection : Elf32ProgbitsSection
    {
        public Elf32SymbolTableSection(Elf32File file)
            : base(file, ".symtab", Elf32SectionType.SHT_SYMTAB, Elf32SectionFlags.SHF_NONE)
        {
            this.Symbols = new Elf32SymbolCollection();
            Symbols.Add(null);
        }

        public Elf32SymbolCollection Symbols { get; private set; }

        public event CreateSymbolHandler OnCreateSymbol;

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

        protected override int Link
        {
            get { return File.Sections.IndexOf(SymbolNames); }
        }

        public Elf32StringTableSection SymbolNames { get; set; }

        public override int EntitySize { get { return Elf32Symbol.SYM_SIZE; } }

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
