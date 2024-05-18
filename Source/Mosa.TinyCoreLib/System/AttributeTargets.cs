namespace System;

[Flags]
public enum AttributeTargets
{
	Assembly = 1,
	Module = 2,
	Class = 4,
	Struct = 8,
	Enum = 0x10,
	Constructor = 0x20,
	Method = 0x40,
	Property = 0x80,
	Field = 0x100,
	Event = 0x200,
	Interface = 0x400,
	Parameter = 0x800,
	Delegate = 0x1000,
	ReturnValue = 0x2000,
	GenericParameter = 0x4000,
	All = 0x7FFF
}
