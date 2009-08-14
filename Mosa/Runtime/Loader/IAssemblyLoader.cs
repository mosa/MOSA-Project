/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System.Collections.Generic;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.Loader
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyLoader
    {
        /// <summary>
        /// Appends the given path to the assembly search path.
        /// </summary>
        /// <param name="path">The path to append to the assembly search path.</param>
        void AppendPrivatePath(string path);

        /// <summary>
        /// Resolves the given assembly reference and loads the associated IMetadataModule.
        /// </summary>
        /// <param name="provider">The metadata provider, which contained the assembly reference.</param>
        /// <param name="assemblyRef">The assembly reference to resolve.</param>
        /// <returns>An instance of IMetadataModule representing the resolved assembly.</returns>
        IMetadataModule Resolve(IMetadataProvider provider, AssemblyRefRow assemblyRef);

        /// <summary>
        /// Loads the named assembly.
        /// </summary>
        /// <param name="file">The file path of the assembly to load.</param>
        /// <returns>The assembly image of the loaded assembly.</returns>
        IMetadataModule Load(string file);

        /// <summary>
        /// Unloads the given module.
        /// </summary>
        /// <param name="module">The module to unload.</param>
        void Unload(IMetadataModule module);

        /// <summary>
        /// Gets an enumerable collection of loaded modules.
        /// </summary>
        IEnumerable<IMetadataModule> Modules { get; }
    }
}
