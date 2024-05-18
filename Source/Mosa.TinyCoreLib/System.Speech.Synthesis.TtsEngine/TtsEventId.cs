namespace System.Speech.Synthesis.TtsEngine;

public enum TtsEventId
{
	StartInputStream = 1,
	EndInputStream,
	VoiceChange,
	Bookmark,
	WordBoundary,
	Phoneme,
	SentenceBoundary,
	Viseme,
	AudioLevel
}
