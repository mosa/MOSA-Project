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

using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.Loader
{
    /// <summary>
    /// Interface of an executable image loaded by a file loader.
    /// </summary>
    public interface IMetadataModule
    {
		/// <summary>
		/// Retrieves the load order index of the module.
		/// </summary>
		int LoadOrder { get; }

		/// <summary>
		/// Retrieves the name of the module.
		/// </summary>
        string Name { get; }

        /// <summary>
        /// Provides access to the provider contained in the assembly.
        /// </summary>
        IMetadataProvider Metadata { get; }

        /// <summary>
        /// Provides access to the sequence of IL opcodes for a relative
        /// virtual address.
        /// </summary>
        /// <param name="rva">The relative virtual address to retrieve a stream for.</param>
        /// <returns>A stream, which represents the relative virtual address.</returns>
        Stream GetInstructionStream(long rva);

        /// <summary>
        /// Gets a stream into the data section, beginning at the specified RVA.
        /// </summary>
        /// <param name="rva">The rva.</param>
        /// <returns>A stream into the data section, pointed to the requested RVA.</returns>
        Stream GetDataSection(long rva);
    }
}
