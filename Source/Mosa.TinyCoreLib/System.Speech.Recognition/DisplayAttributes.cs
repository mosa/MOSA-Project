namespace System.Speech.Recognition;

[Flags]
public enum DisplayAttributes
{
	None = 0,
	ZeroTrailingSpaces = 2,
	OneTrailingSpace = 4,
	TwoTrailingSpaces = 8,
	ConsumeLeadingSpaces = 0x10
}
