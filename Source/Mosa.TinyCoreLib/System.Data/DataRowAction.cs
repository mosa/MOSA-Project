namespace System.Data;

[Flags]
public enum DataRowAction
{
	Nothing = 0,
	Delete = 1,
	Change = 2,
	Rollback = 4,
	Commit = 8,
	Add = 0x10,
	ChangeOriginal = 0x20,
	ChangeCurrentAndOriginal = 0x40
}
