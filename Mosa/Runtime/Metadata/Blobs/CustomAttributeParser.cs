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
using Mosa.Runtime.Loader;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.Metadata.Blobs
{
    /// <summary>
    /// Parses and instantiates custom attributes in assembly metadata blobs.
    /// </summary>
    public static class CustomAttributeParser
    {
        #region Methods

        /// <summary>
        /// Parses the specified attribute blob and instantiates the attribute.
        /// </summary>
        /// <param name="module">The metadata module, which contains the attribute blob.</param>
        /// <param name="attributeBlob">The attribute blob token.</param>
        /// <param name="attributeCtor">The constructor of the attribute.</param>
        /// <returns></returns>
        public static object Parse(IMetadataModule module, TokenTypes attributeBlob, RuntimeMethod attributeCtor)
        {
            return null;
        }

        #endregion // Methods
    }
}
