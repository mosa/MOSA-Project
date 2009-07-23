using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.Compiler.Metadata
{
    /// <summary>
    /// Stores positional information about a metadata stream.
    /// </summary>
    public struct MetadataStreamPosition
    {
        /// <summary>
        /// Holds the position of the stream relative to the start of the metadata symbol stream.
        /// </summary>
        public long Position;

        /// <summary>
        /// Holds the size of the stream in byes.
        /// </summary>
        public long Size;
    }
}
