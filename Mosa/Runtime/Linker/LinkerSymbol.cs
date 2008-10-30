/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *
 */

using System;
using System.Diagnostics;

namespace Mosa.Runtime.Linker
{
    /// <summary>
    /// Represents a single symbol for the linker.
    /// </summary>
    public sealed class LinkerSymbol
    {
        #region Data members

        /// <summary>
        /// Holds the address of the symbol.
        /// </summary>
        private IntPtr address;

        /// <summary>
        /// Holds the length of the linker symbol in bytes.
        /// </summary>
        private long length;

        /// <summary>
        /// Holds the name of the linker symbol.
        /// </summary>
        private string name;

        /// <summary>
        /// Holds the section containing the linker symbol.
        /// </summary>
        private SectionKind section;

        /// <summary>
        /// The section start relative address.
        /// </summary>
        private long sectionAddress;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkerSymbol"/> class.
        /// </summary>
        /// <param name="name">The name of the symbol.</param>
        /// <param name="section">The section holding the symbol.</param>
        /// <param name="address">Holds the address of the symbol.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="name"/> is empty.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="name"/> is null.</exception>
        public LinkerSymbol(string name, SectionKind section, long address)
        {
            Debug.Assert(false == String.IsNullOrEmpty(name), @"LinkerSymbol requires a proper name.");
            if (name == null)
                throw new ArgumentNullException(@"name");
            if (name.Length == 0)
                throw new ArgumentException(@"Name can't be empty.", @"name");

            this.name = name;
            this.section = section;
            this.sectionAddress = address;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the address of the linker symbol.
        /// </summary>
        /// <value>The address of the linker symbol.</value>
        public IntPtr Address
        {
            get { return this.address; }
            internal set { this.address = value; }
        }

        /// <summary>
        /// Gets or sets the length of the linker symbol in bytes.
        /// </summary>
        /// <value>The length in bytes.</value>
        public long Length
        {
            get { return this.length; }
            internal set { this.length = value; }
        }

        /// <summary>
        /// Retrieves the symbol name.
        /// </summary>
        /// <value>The name of the linker symbol.</value>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the section holding the symbol.
        /// </summary>
        /// <value>The section.</value>
        public SectionKind Section
        {
            get { return this.section; }
        }

        /// <summary>
        /// Gets the section start relative address of the symbol.
        /// </summary>
        /// <value>The section start relative address.</value>
        public long SectionAddress
        {
            get { return this.sectionAddress; }
        }

        #endregion // Properties
    }
}
