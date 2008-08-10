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
    public enum CustomModType
    {
        None = 0,
        Optional = 1,
        Required = 2
    }

    public struct CustomMod
    {
        private CustomModType _type;
        private TokenTypes _token;

        public CustomMod(CustomModType type, TokenTypes token)
        {
            _type = type;
            _token = token;
        }

        public CustomModType Type { get { return _type; } }
        public TokenTypes Token { get { return _token; } }

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
