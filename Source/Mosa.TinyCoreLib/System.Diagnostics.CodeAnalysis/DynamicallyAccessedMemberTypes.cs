namespace System.Diagnostics.CodeAnalysis;

[Flags]
public enum DynamicallyAccessedMemberTypes
{
	All = -1,
	None = 0,
	PublicParameterlessConstructor = 1,
	PublicConstructors = 3,
	NonPublicConstructors = 4,
	PublicMethods = 8,
	NonPublicMethods = 0x10,
	PublicFields = 0x20,
	NonPublicFields = 0x40,
	PublicNestedTypes = 0x80,
	NonPublicNestedTypes = 0x100,
	PublicProperties = 0x200,
	NonPublicProperties = 0x400,
	PublicEvents = 0x800,
	NonPublicEvents = 0x1000,
	Interfaces = 0x2000
}
