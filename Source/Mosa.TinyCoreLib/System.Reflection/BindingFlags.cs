namespace System.Reflection;

[Flags]
public enum BindingFlags
{
	Default = 0,
	IgnoreCase = 1,
	DeclaredOnly = 2,
	Instance = 4,
	Static = 8,
	Public = 0x10,
	NonPublic = 0x20,
	FlattenHierarchy = 0x40,
	InvokeMethod = 0x100,
	CreateInstance = 0x200,
	GetField = 0x400,
	SetField = 0x800,
	GetProperty = 0x1000,
	SetProperty = 0x2000,
	PutDispProperty = 0x4000,
	PutRefDispProperty = 0x8000,
	ExactBinding = 0x10000,
	SuppressChangeType = 0x20000,
	OptionalParamBinding = 0x40000,
	IgnoreReturn = 0x1000000,
	DoNotWrapExceptions = 0x2000000
}
