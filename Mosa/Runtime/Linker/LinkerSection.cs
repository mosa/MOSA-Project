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

namespace Mosa.Runtime.Linker
{
    /// <summary>
    /// Abstract class, that represents sections in an executable file provided by the linker.
    /// </summary>
    public abstract class LinkerSection
    {
        #region Data members

        /// <summary>
        /// Holds the sections load address.
        /// </summary>
        private IntPtr address;

        /// <summary>
        /// Holds the kind of the section.
        /// </summary>
        private SectionKind kind;

        /// <summary>
        /// Holds the file offset of this section.
        /// </summary>
        private long offset;

        /// <summary>
        /// Holds the section name.
        /// </summary>
        private string name;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkerSection"/> class.
        /// </summary>
        /// <param name="kind">The kind of the section.</param>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        protected LinkerSection(SectionKind kind, string name, IntPtr address)
        {
            this.address = address;
            this.kind = kind;
            this.name = name;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public IntPtr Address
        {
            get { return this.address; }
            internal set { this.address = value; }
        }

        /// <summary>
        /// Gets the length of the section in bytes.
        /// </summary>
        /// <value>The length of the section in bytes.</value>
        public abstract long Length
        {
            get;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets or sets the file offset of the section.
        /// </summary>
        /// <value>The file offset.</value>
        public long Offset
        {
            get { return this.offset; }
            internal set { this.offset = value; }
        }

        /// <summary>
        /// Gets the kind of the section.
        /// </summary>
        /// <value>The kind of the section.</value>
        public SectionKind SectionKind
        {
            get { return this.kind; }
        }

        #endregion // Properties
    }
}
