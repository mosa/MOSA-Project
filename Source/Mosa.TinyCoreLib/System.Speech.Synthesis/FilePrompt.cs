namespace System.Speech.Synthesis;

public class FilePrompt : Prompt
{
	public FilePrompt(string path, SynthesisMediaType media)
		: base((string)null)
	{
	}

	public FilePrompt(Uri promptFile, SynthesisMediaType media)
		: base((string)null)
	{
	}
}
