/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

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
				this.Token = new ScopedToken(module, definition.MDToken);
			this.Definition = definition;
			this.Signature = signature;
		}

		public UnitDesc<Def, Sig> Clone(Sig newSig)
		{
			UnitDesc<Def, Sig> result = (UnitDesc<Def, Sig>)base.MemberwiseClone();
			result.Signature = newSig;
			return result;
		}
	}
}
