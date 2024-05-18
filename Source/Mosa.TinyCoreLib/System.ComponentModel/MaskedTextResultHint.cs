namespace System.ComponentModel;

public enum MaskedTextResultHint
{
	PositionOutOfRange = -55,
	NonEditPosition = -54,
	UnavailableEditPosition = -53,
	PromptCharNotAllowed = -52,
	InvalidInput = -51,
	SignedDigitExpected = -5,
	LetterExpected = -4,
	DigitExpected = -3,
	AlphanumericCharacterExpected = -2,
	AsciiCharacterExpected = -1,
	Unknown = 0,
	CharacterEscaped = 1,
	NoEffect = 2,
	SideEffect = 3,
	Success = 4
}
