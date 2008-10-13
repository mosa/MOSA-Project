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
using Mosa.Runtime.Metadata.Tables;
using System.Diagnostics;

namespace Mosa.Runtime.Metadata.Signatures
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalVariableSignature : Signature
    {
        #region Data members

        /// <summary>
        /// Holds the signature types of all local variables in order of definition.
        /// </summary>
        private SigType[] _types;

        /// <summary>
        /// A shared empty array for those signatures, who do not have local variables.
        /// </summary>
        private static SigType[] Empty = new SigType[0];

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalVariableSignature"/> class.
        /// </summary>
        public LocalVariableSignature()
        {
            _types = LocalVariableSignature.Empty;
        }

        #endregion // Construction


        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <value>The types.</value>
        public SigType[] Types
        {
            get { return _types; }
        }

        /// <summary>
        /// Parses the signature.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        protected override void ParseSignature(byte[] buffer, ref int index)
        {
            // Check signature identifier
            if (buffer[index++] != 0x07)
                throw new ArgumentException(@"Token doesn't represent a local variable signature.", @"token");

            // Retrieve the number of locals
            int count = Utilities.ReadCompressedInt32(buffer, ref index);
            if (0 != count)
            {
                _types = new SigType[count];
                for (int i = 0; i < count; i++)
                {
                    _types[i] = SigType.ParseTypeSignature(buffer, ref index);
                }
            }
        }

        /// <summary>
        /// Parses the specified provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public static LocalVariableSignature Parse(IMetadataProvider provider, TokenTypes token)
        {
            byte[] buffer;
            int index = 0;
            provider.Read(token, out buffer);
            LocalVariableSignature sig = new LocalVariableSignature();
            sig.ParseSignature(buffer, ref index);
            Debug.Assert(index == buffer.Length, @"Signature parser didn't complete.");
            return sig;
        }
    }
}
