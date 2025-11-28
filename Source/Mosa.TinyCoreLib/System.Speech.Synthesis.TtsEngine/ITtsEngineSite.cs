using System.IO;

namespace System.Speech.Synthesis.TtsEngine;

public interface ITtsEngineSite
{
	int Actions { get; }

	int EventInterest { get; }

	int Rate { get; }

	int Volume { get; }

	void AddEvents(SpeechEventInfo[] events, int count);

	void CompleteSkip(int skipped);

	SkipInfo GetSkipInfo();

	Stream LoadResource(Uri uri, string mediaType);

	int Write(IntPtr data, int count);
}
