using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.Linker
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
        }

        /// <summary>
        /// Gets the length of the section in bytes.
        /// </summary>
        /// <value>The length of the section in bytes.</value>
        public abstract ulong Length
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
