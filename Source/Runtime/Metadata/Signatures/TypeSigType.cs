
using System;

namespace Mosa.Runtime.Metadata.Signatures
{
    public class TypeSigType : SigType
    {
        private readonly TokenTypes token;

        public TokenTypes Token { get { return this.token; } }

        public TypeSigType(TokenTypes token, CilElementType type) :
            base(type)
        {
            this.token = token;
        }

    }
}
