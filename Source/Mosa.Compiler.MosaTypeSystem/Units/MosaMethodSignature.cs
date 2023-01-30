// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.MosaTypeSystem;

public class MosaMethodSignature : IEquatable<MosaMethodSignature>
{
	public MosaType? ReturnType { get; }

	public IList<MosaParameter> Parameters { get; }

	public MosaMethodSignature(MosaType? returnType, IList<MosaParameter> parameter)
	{
		ReturnType = returnType;
		Parameters = new List<MosaParameter>(parameter).AsReadOnly();
	}

	public bool Equals(MosaMethodSignature? sig)
	{
		return SignatureComparer.Equals(ReturnType, sig?.ReturnType) &&
			   Parameters.SequenceEquals(sig?.Parameters);
	}

	private string? sig;

	public override string ToString()
	{
		return sig ??= SignatureName.GetSignature("", this, true);
	}
}
