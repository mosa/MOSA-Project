namespace System.Data;

[Flags]
public enum CommandBehavior
{
	Default = 0,
	SingleResult = 1,
	SchemaOnly = 2,
	KeyInfo = 4,
	SingleRow = 8,
	SequentialAccess = 0x10,
	CloseConnection = 0x20
}
