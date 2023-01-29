// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Inline Method Data
/// </summary>
public sealed class InlineMethodData
{
	public BasicBlocks BasicBlocks { get; }

	public int Version { get; }

	public HashSet<MosaMethod> References { get; private set; }

	public bool IsInlined { get { return BasicBlocks != null; } }

	public InlineMethodData(BasicBlocks basicBlocks, int version)
	{
		References = new HashSet<MosaMethod>();
		BasicBlocks = basicBlocks;
		Version = version;
	}

	public void AddReference(MosaMethod method)
	{
		if (!References.Contains(method))
		{
			References.Add(method);
		}
	}
}
