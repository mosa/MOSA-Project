// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;

namespace Mosa.Compiler.MosaTypeSystem.Metadata
{
	internal class UnitDesc<Def, Sig> where Def : IMDTokenProvider
	{
		public ScopedToken Token { get; private set; }

		public Def Definition { get; private set; }

		public Sig Signature { get; private set; }

		public UnitDesc(ModuleDef module, Def definition, Sig signature)
		{
			if (definition != null)
				Token = new ScopedToken(module, definition.MDToken);
			Definition = definition;
			Signature = signature;
		}

		public UnitDesc<Def, Sig> Clone(Sig newSig)
		{
			UnitDesc<Def, Sig> result = (UnitDesc<Def, Sig>)base.MemberwiseClone();
			result.Signature = newSig;
			return result;
		}
	}
}
