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
    public sealed class FieldSignature : Signature
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
        /// Initializes a new instance of the <see cref="FieldSignature"/> class.
        /// </summary>
        public FieldSignature()
        {
        }

        /// <summary>
        /// Gets the custom mods.
        /// </summary>
        /// <value>The custom mods.</value>
        public CustomMod[] CustomMods
        {
            get { return _customMods; }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public SigType Type
        {
            get { return _type; }
        }


        /// <summary>
        /// Parses the signature.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        protected override void ParseSignature(byte[] buffer, ref int index)
        {
            if (FIELD == buffer[index])
            {
                index++;
                _customMods = CustomMod.ParseCustomMods(buffer, ref index);
                _type = SigType.ParseTypeSignature(buffer, ref index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private const int FIELD = 0x06;
    }
}
