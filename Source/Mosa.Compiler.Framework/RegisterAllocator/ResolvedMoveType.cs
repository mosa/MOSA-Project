// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator;

public enum ResolvedMoveType
{
	Invalid,
	Move,
	Exchange,
	Load,
	Spill, // Store
}
