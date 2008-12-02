/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon "Kintaro" Wollwage (<mailto:kintaro@mosa-project.org>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.ObjectFiles.Elf32
{
    /// <summary>
    /// 
    /// </summary>
    public class Elf32Section : Mosa.Runtime.Linker.LinkerSection
    {
        /// <summary>
        /// 
        /// </summary>
        protected long length = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32Section"/> class.
        /// </summary>
        /// <param name="kind">The kind of the section.</param>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        public Elf32Section(Mosa.Runtime.Linker.SectionKind kind, string name, IntPtr address) 
            : base(kind, name, address)
        {
        }

        /// <summary>
        /// Gets the length of the section in bytes.
        /// </summary>
        /// <value>The length of the section in bytes.</value>
        public override long Length
        {
            get 
            {
                return length;
            }
        }
    }
}
