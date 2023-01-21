// Copyright (c) MOSA Project. Licensed under the New BSD License.

using dnlib.DotNet;

namespace Mosa.Compiler.MosaTypeSystem.CLR.Utils
{
	internal class UnitDesc<TDef, TSig> where TDef : IMDTokenProvider
	{
		public ScopedToken Token { get; }

		public TDef? Definition { get; }

		public TSig? Signature { get; private set; }

		public UnitDesc(ModuleDef? module, TDef? definition, TSig? signature)
		{
			if (definition != null)
				Token = new ScopedToken(module, definition.MDToken);
			Definition = definition;
			Signature = signature;
		}

		public UnitDesc<TDef, TSig> Clone(TSig? newSig)
		{
			var result = (UnitDesc<TDef, TSig>)MemberwiseClone();
			result.Signature = newSig;
			return result;
		}
	}
}
