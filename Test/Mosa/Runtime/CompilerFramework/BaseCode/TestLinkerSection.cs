using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework.Linker;
using System.IO;
using Mosa.Runtime;

namespace Test.Mosa.Runtime.CompilerFramework.BaseCode
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TestLinkerSection : LinkerSection
    {
        #region Data members

        /// <summary>
        /// Holds the stream of this linker section.
        /// </summary>
        private Stream stream;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLinkerSection"/> class.
        /// </summary>
        /// <param name="kind">The kind of the section.</param>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        public TestLinkerSection(SectionKind kind, string name, IntPtr address) :
            base(kind, name, address)
        {
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Allocates a stream of the specified size from the section.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="alignment">The alignment.</param>
        /// <returns></returns>
        public Stream Allocate(int size, int alignment)
        {
            Stream stream = this.stream;
            if (null == stream || (size != 0 && size > (stream.Length - stream.Position)))
            {
                // Request 64K of memory
                stream = new VirtualMemoryStream(RuntimeBase.Instance.MemoryManager, 16 * 4096);

                // FIXME: Put the old stream into a list to dispose

                // Save the stream for further references
                this.stream = stream;
            }

            return stream;
        }

        #endregion // Methods

        #region LinkerSection Overrides

        /// <summary>
        /// Gets the length of the section in bytes.
        /// </summary>
        /// <value>The length of the section in bytes.</value>
        public override ulong Length
        {
            get { throw new NotImplementedException(); }
        }

        #endregion // LinkerSection Overrides
    }
}
