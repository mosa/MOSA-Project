namespace System.Data.Common;

[Flags]
public enum SupportedJoinOperators
{
	None = 0,
	Inner = 1,
	LeftOuter = 2,
	RightOuter = 4,
	FullOuter = 8
}
