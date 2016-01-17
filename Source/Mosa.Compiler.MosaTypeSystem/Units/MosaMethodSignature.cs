// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;

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
			return SignatureComparer.Equals(ReturnType, sig.ReturnType) &&
				   Parameters.SequenceEquals(sig.Parameters);
		}

		private string sig;

		public override string ToString()
		{
			return sig ?? (sig = SignatureName.GetSignature("", this, true));
		}
	}
}
