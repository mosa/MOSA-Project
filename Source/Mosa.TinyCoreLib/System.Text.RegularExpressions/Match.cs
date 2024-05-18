namespace System.Text.RegularExpressions;

public class Match : Group
{
	public static Match Empty
	{
		get
		{
			throw null;
		}
	}

	public virtual GroupCollection Groups
	{
		get
		{
			throw null;
		}
	}

	internal Match()
	{
	}

	public Match NextMatch()
	{
		throw null;
	}

	public virtual string Result(string replacement)
	{
		throw null;
	}

	public static Match Synchronized(Match inner)
	{
		throw null;
	}
}
