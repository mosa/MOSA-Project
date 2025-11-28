namespace System.Speech.Synthesis.TtsEngine;

public abstract class TtsEngineSsml
{
	protected TtsEngineSsml(string registryKey)
	{
	}

	public abstract void AddLexicon(Uri uri, string mediaType, ITtsEngineSite site);

	public abstract IntPtr GetOutputFormat(SpeakOutputFormat speakOutputFormat, IntPtr targetWaveFormat);

	public abstract void RemoveLexicon(Uri uri, ITtsEngineSite site);

	public abstract void Speak(TextFragment[] fragment, IntPtr waveHeader, ITtsEngineSite site);
}
