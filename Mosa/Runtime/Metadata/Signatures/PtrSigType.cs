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

namespace Mosa.Runtime.Metadata.Signatures
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PtrSigType : SigType
    {
        /// <summary>
        /// 
        /// </summary>
        private CustomMod[] _customMods;
        /// <summary>
        /// 
        /// </summary>
        private SigType _type;

        /// <summary>
        /// Initializes a new instance of the <see cref="PtrSigType"/> class.
        /// </summary>
        /// <param name="customMods">The custom mods.</param>
        /// <param name="type">The type.</param>
        public PtrSigType(CustomMod[] customMods, SigType type)
            : base(CilElementType.Ptr)
        {
            _customMods = customMods;
            _type = type;
        }

        /// <summary>
        /// Gets the custom mods.
        /// </summary>
        /// <value>The custom mods.</value>
        public CustomMod[] CustomMods { get { return _customMods; } }
        /// <summary>
        /// Gets the type of the element.
        /// </summary>
        /// <value>The type of the element.</value>
        public SigType ElementType { get { return _type; } }
    }
}
