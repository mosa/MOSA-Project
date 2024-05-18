namespace System.Security.Policy;

[Flags]
public enum PolicyStatementAttribute
{
	Nothing = 0,
	Exclusive = 1,
	LevelFinal = 2,
	All = 3
}
