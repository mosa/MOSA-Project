using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Linker.Elf32
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32SymbolTableEntry
    {
        /// <summary>
        /// This member holds an index into the object file's symbol string table, which holds 
        /// the character representations of the symbol names.
        /// </summary>
        public uint Name;
        /// <summary>
        /// This member gives the value of the associated symbol. Depending on the context, 
        /// this may be an absolute value, an virtualAddress, and so on; details appear below. 
        /// </summary>
        public uint Value;
        /// <summary>
        /// Many symbols have associated sizes. For example, a data object's size is the number 
        /// of bytes contained in the object. This member holds 0 if the symbol has no size or 
        /// an unknown size. 
        /// </summary>
        public uint Size;
        /// <summary>
        /// This member specifies the symbol's type and binding attributes. A list of the values 
        /// and meanings appears below. The following code shows how to manipulate the 
        /// values. 
        /// </summary>
        public byte Info;
        /// <summary>
        /// This member currently holds 0 and has no defined meaning. 
        /// </summary>
        public byte Other;
        /// <summary>
        /// Every symbol table entry is "defined'' in relation to some section; this member holds 
        /// the relevant section header table index.
        /// </summary>
        public ushort SectionHeaderTableIndex;
    }
}
