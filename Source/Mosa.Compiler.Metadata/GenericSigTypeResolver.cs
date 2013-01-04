using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Metadata
{
    public static class GenericSigTypeResolver
    {

        public static SigType Resolve(SigType sigType, SigType[] genericArguments)
        {
            if (genericArguments == null)
                return sigType;

            if (sigType is VarSigType)
            {
                if ((sigType as VarSigType).Index < genericArguments.Length)
                    return genericArguments[(sigType as VarSigType).Index];
            }

            return sigType;
        }

        public static SigType[] Resolve(SigType[] sigTypes, SigType[] genericArguments)
        {
            SigType[] newSigTypes = new SigType[sigTypes.Length];

            for (int i = 0; i < sigTypes.Length; i++)
            {
                newSigTypes[i] = Resolve(sigTypes[i], genericArguments);
            }

            return newSigTypes;
        }
    }
}
