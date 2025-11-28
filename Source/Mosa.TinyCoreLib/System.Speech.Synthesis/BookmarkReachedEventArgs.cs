namespace System.Speech.Synthesis;

public class BookmarkReachedEventArgs : PromptEventArgs
{
	public TimeSpan AudioPosition
	{
		get
		{
			throw null;
		}
	}

	public string Bookmark
	{
		get
		{
			throw null;
		}
	}

	internal BookmarkReachedEventArgs()
	{
	}
}
