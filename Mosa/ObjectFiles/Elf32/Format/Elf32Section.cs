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

namespace Mosa.ObjectFiles.Elf32.Format
{
    /// <summary>
    /// Sections contain all information in an object file, except the ELF header, the program header 
    /// table, and the section header table. Moreover, object files' sections satisfy several conditions. 
    /// </summary>
    abstract class Elf32Section
    {
        #region Members
        /// <summary>
        /// This member specifies the name of the section. Its value is an index into 
        /// the section header string table section [see "String Table'' below], giving 
        /// the location of a null-terminated string. 
        /// </summary>
        string _name;

        /// <summary>
        /// This member categorizes the section's contents and semantics. Section 
        /// types and their descriptions appear below.
        /// </summary>
        Elf32SectionType _type;

        /// <summary>
        /// Sections support 1-bit flags that describe miscellaneous attributes. Flag 
        /// definitions appear below. 
        /// </summary>
        Elf32SectionFlags _flags;

        /// <summary>
        /// A reference to the objectfile this section belongs to
        /// </summary>
        Elf32File _file;

        /// <summary>
        /// This member gives the section's size in bytes.  Unless the section type is 
        /// SHT_NOBITS, the section occupies sh_size bytes in the file. A section 
        /// of type SHT_NOBITS may have a non-zero size, but it occupies no space 
        /// in the file. 
        /// </summary>
        public const int SHDR_SIZE = 40;
        #endregion

        #region Construction
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">File to write to</param>
        /// <param name="name">The section's name</param>
        /// <param name="type">Sectiontype</param>
        /// <param name="flags">Flags to use for this section</param>
        public Elf32Section(Elf32File file, string name, Elf32SectionType type, Elf32SectionFlags flags)
        {
            _name = name;
            _type = type;
            _flags = flags;
            _file = file;
            file.Sections.Add(this);
        }
        #endregion

        #region Properties
        /// <summary>
        /// A reference to the objectfile this section belongs to
        /// </summary>
        protected Elf32File File 
        { 
            get 
            { 
                return _file; 
            } 
        }

        /// <summary>
        /// This member specifies the name of the section. Its value is an index into 
        /// the section header string table section [see "String Table'' below], giving 
        /// the location of a null-terminated string. 
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// 
        /// </summary>
        public Elf32SectionType Type 
        { 
            get 
            { 
                return _type; 
            } 
        }

        /// <summary>
        /// Sections support 1-bit flags that describe miscellaneous attributes. Flag 
        /// definitions appear below. 
        /// </summary>
        public Elf32SectionFlags Flags 
        { 
            get 
            { 
                return _flags; 
            } 
        }

        /// <summary>
        /// If the section will appear in the memory image of a process, this member 
        /// gives the address at which the section's first byte should reside. Otherwise, 
        /// the member contains 0. 
        /// </summary>
        public virtual IntPtr RuntimeImageAddress 
        { 
            get 
            { 
                return IntPtr.Zero; 
            } 
        }

        /// <summary>
        /// Some sections hold a table of fixed-size entries, such as a symbol table. For 
        /// such a section, this member gives the size in bytes of each entry. The 
        /// member contains 0 if the section does not hold a table of fixed-size entries.
        /// </summary>
        public virtual int EntitySize 
        { 
            get 
            { 
                return 0; 
            } 
        }

        /// <summary>
        /// This member's value gives the byte offset from the beginning of the file to 
        /// the first byte in the section. One section type, SHT_NOBITS described 
        /// below, occupies no space in the file, and its sh_offset member locates 
        /// the conceptual placement in the file. 
        /// </summary>
        public abstract int Offset 
        { 
            get; 
        }

        /// <summary>
        /// This member gives the section's size in bytes.  Unless the section type is 
        /// SHT_NOBITS, the section occupies sh_size bytes in the file. A section 
        /// of type SHT_NOBITS may have a non-zero size, but it occupies no space 
        /// in the file. 
        /// </summary>
        public abstract int Size 
        { 
            get; 
        }

        /// <summary>
        /// This member holds a section header table index link, whose interpretation 
        /// depends on the section type. A table below describes the values. 
        /// </summary>
        protected virtual int Link 
        { 
            get 
            { 
                return 0; 
            } 
        }

        /// <summary>
        /// This member holds extra information, whose interpretation depends on the 
        /// section type. A table below describes the values. 
        /// </summary>
        protected virtual int Info 
        { 
            get 
            { 
                return 0; 
            } 
        }

        /// <summary>
        /// Some sections have address alignment constraints. For example, if a section 
        /// holds a doubleword, the system must ensure doubleword alignment for the 
        /// entire section.  That is, the value of sh_addr must be congruent to 0, 
        /// modulo the value of sh_addralign. Currently, only 0 and positive 
        /// integral powers of two are allowed. Values 0 and 1 mean the section has no 
        /// alignment constraints. 
        /// </summary>
        protected virtual int AddrAlign 
        { 
            get 
            { 
                return 0; 
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public virtual void WriteHeader(BinaryWriter writer)
        {
            writer.Write((Int32)File.SectionNames.GetStringIndex(Name));
            writer.Write((Int32)Type);
            writer.Write((Int32)Flags);
            writer.Write(RuntimeImageAddress.ToInt32());
            writer.Write((Int32)Offset);
            writer.Write((Int32)Size);
            writer.Write((Int32)Link); // sh_link
            writer.Write((Int32)Info); // sh_info
            writer.Write((Int32)AddrAlign); // sh_addralign
            writer.Write((Int32)EntitySize); // sh_entsize
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public abstract void WriteData(BinaryWriter writer);
        #endregion
    }
}
