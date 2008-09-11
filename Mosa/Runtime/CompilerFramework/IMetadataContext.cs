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
using System.IO;
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMetadataContext
    {
        #region Properties

        /// <summary>
        /// Returns the provider provider of the object.
        /// </summary>
        IMetadataProvider Provider { get; }

        /// <summary>
        /// Returns the token associated with this instance.
        /// </summary>
        TokenTypes Token { get; }

        #endregion // Properties
    }
}
