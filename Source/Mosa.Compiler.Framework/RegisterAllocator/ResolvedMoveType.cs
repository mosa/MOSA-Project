// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator;

public enum ResolvedMoveType
{
	Invalid,
	ConstantLoad,
	Move,
	Exchange,
	Load,
	Spill, // Store
}
