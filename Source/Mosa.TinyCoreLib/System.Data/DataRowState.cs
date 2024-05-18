namespace System.Data;

[Flags]
public enum DataRowState
{
	Detached = 1,
	Unchanged = 2,
	Added = 4,
	Deleted = 8,
	Modified = 0x10
}
