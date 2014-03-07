/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Ki (kiootic) <kiootic@gmail.com>
 */

using System;
using System.Collections.Generic;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaMethodSignature : IEquatable<MosaMethodSignature>
	{
		public MosaType ReturnType { get; private set; }
		public IList<MosaParameter> Parameters { get; private set; }

		public MosaMethodSignature(MosaType returnType, IList<MosaParameter> parameter)
		{
			ReturnType = returnType;
			Parameters = new List<MosaParameter>(parameter).AsReadOnly();
		}

		public bool Equals(MosaMethodSignature sig)
		{
			return SignatureComparer.Equals(this.ReturnType, sig.ReturnType) &&
				   this.Parameters.SequenceEquals(sig.Parameters);
		}

		string sig;
		public override string ToString()
		{
			return sig ?? (sig = SignatureName.GetSignature("", this, true));
		}
	}
}
