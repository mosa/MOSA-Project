namespace System.Runtime.Versioning;

[Flags]
public enum ComponentGuaranteesOptions
{
	None = 0,
	Exchange = 1,
	Stable = 2,
	SideBySide = 4
}
