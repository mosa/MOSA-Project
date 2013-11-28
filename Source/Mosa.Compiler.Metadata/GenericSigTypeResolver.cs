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
			else if (sigType is GenericInstSigType)
			{
				var genericInstSigType = sigType as GenericInstSigType;
				for (var i = 0; i < genericInstSigType.GenericArguments.Length; ++i)
				{
					if (genericInstSigType.GenericArguments[i] is VarSigType)
						return genericArguments[(genericInstSigType.GenericArguments[i] as VarSigType).Index];
				}
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