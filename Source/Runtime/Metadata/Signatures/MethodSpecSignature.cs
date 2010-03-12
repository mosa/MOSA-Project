/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata.Signatures
{
    /// <summary>
    /// 
    /// </summary>
    public class MethodSpecSignature : Signature, ISignatureContext
    {
        private readonly ISignatureContext outerContext;

        /// <summary>
        /// 
        /// </summary>
        private SigType[] types;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodSpecSignature"/> class.
        /// </summary>
        public MethodSpecSignature(ISignatureContext outerContext)
        {
            this.outerContext = outerContext;
        }

        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <value>The types.</value>
        public SigType[] Types 
        { 
            get { return types; } 
        }

        /// <summary>
        /// Parses the signature.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        protected override void ParseSignature(ISignatureContext context, byte[] buffer, ref int index)
        {
            if (0x0A != buffer[index])
                throw new InvalidOperationException(@"Invalid signature.");
            index++;

            int genArgCount = Utilities.ReadCompressedInt32(buffer, ref index);
            this.types = new SigType[genArgCount];
            for (int i = 0; i < genArgCount; i++)
                this.types[i] = SigType.ParseTypeSignature(context, buffer, ref index);
        }

        public SigType GetGenericMethodArgument(int index)
        {
            return this.types[index];
        }

        public SigType GetGenericTypeArgument(int index)
        {
            return this.outerContext.GetGenericTypeArgument(index);
        }
    }
}
