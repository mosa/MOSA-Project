/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.IO;

namespace Mosa.Runtime.Linker.PE
{
    /// <summary>
    /// An implementation of a portable executable linker section.
    /// </summary>
    public class PortableExecutableLinkerSection : LinkerSection
    {
        #region Data members

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PortableExecutableLinkerSection"/> class.
        /// </summary>
        /// <param name="kind">The kind of the section.</param>
        /// <param name="name">The name of the section.</param>
        /// <param name="address">The address of the section.</param>
        public PortableExecutableLinkerSection(SectionKind kind, string name, IntPtr address) : 
            base(kind, name, address)
        {
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Allocates the specified number of bytes in the section.
        /// </summary>
        /// <param name="size">The number of bytes to allocate.</param>
        /// <param name="alignment">The alignment.</param>
        /// <returns>A stream, used to write to the allocated memory region.</returns>
        public Stream Allocate(int size, int alignment)
        {
            return Stream.Null;
        }

        #endregion // Methods

        #region LinkerSection Overrides

        /// <summary>
        /// Gets the length of the section in bytes.
        /// </summary>
        /// <value>The length of the section in bytes.</value>
        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        #endregion // LinkerSection Overrides
    }
}
