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
    public enum CustomModType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Optional = 1,
        /// <summary>
        /// 
        /// </summary>
        Required = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public struct CustomMod
    {
        /// <summary>
        /// 
        /// </summary>
        private CustomModType _type;
        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _token;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomMod"/> struct.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="token">The token.</param>
        public CustomMod(CustomModType type, TokenTypes token)
        {
            _type = type;
            _token = token;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public CustomModType Type { get { return _type; } }
        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <value>The token.</value>
        public TokenTypes Token { get { return _token; } }

        /// <summary>
        /// Parses the custom mods.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static CustomMod[] ParseCustomMods(byte[] buffer, ref int index)
        {
            List<CustomMod> mods = new List<CustomMod>();
            for (int i = index; i < buffer.Length; i++)
            {
                CilElementType type = (CilElementType)buffer[i++];
                if (type != CilElementType.Optional && type != CilElementType.Required)
                    break;

                index++;
                TokenTypes modType = SigType.ReadTypeDefOrRefEncoded(buffer, ref index);
                mods.Add(new CustomMod((CustomModType)(type - CilElementType.Required + 1), modType));
            }

            return mods.ToArray();
        }
    }
}
