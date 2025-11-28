using System.ComponentModel;

namespace System.Text.RegularExpressions;

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class RegexRunner
{
	protected internal int[]? runcrawl;

	protected internal int runcrawlpos;

	protected internal Match? runmatch;

	protected internal Regex? runregex;

	protected internal int[]? runstack;

	protected internal int runstackpos;

	protected internal string? runtext;

	protected internal int runtextbeg;

	protected internal int runtextend;

	protected internal int runtextpos;

	protected internal int runtextstart;

	protected internal int[]? runtrack;

	protected internal int runtrackcount;

	protected internal int runtrackpos;

	protected internal RegexRunner()
	{
	}

	protected void Capture(int capnum, int start, int end)
	{
	}

	public static bool CharInClass(char ch, string charClass)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete mechanisms for Regex extensibility. It is not supported.", DiagnosticId = "SYSLIB0052", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected static bool CharInSet(char ch, string set, string category)
	{
		throw null;
	}

	protected void CheckTimeout()
	{
	}

	protected void Crawl(int i)
	{
	}

	protected int Crawlpos()
	{
		throw null;
	}

	protected void DoubleCrawl()
	{
	}

	protected void DoubleStack()
	{
	}

	protected void DoubleTrack()
	{
	}

	protected void EnsureStorage()
	{
	}

	protected virtual bool FindFirstChar()
	{
		throw null;
	}

	protected virtual void Go()
	{
		throw null;
	}

	protected virtual void InitTrackCount()
	{
		throw null;
	}

	protected bool IsBoundary(int index, int startpos, int endpos)
	{
		throw null;
	}

	protected bool IsECMABoundary(int index, int startpos, int endpos)
	{
		throw null;
	}

	protected bool IsMatched(int cap)
	{
		throw null;
	}

	protected int MatchIndex(int cap)
	{
		throw null;
	}

	protected int MatchLength(int cap)
	{
		throw null;
	}

	protected int Popcrawl()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete mechanisms for Regex extensibility. It is not supported.", DiagnosticId = "SYSLIB0052", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected internal Match? Scan(Regex regex, string text, int textbeg, int textend, int textstart, int prevlen, bool quick)
	{
		throw null;
	}

	[Obsolete("This API supports obsolete mechanisms for Regex extensibility. It is not supported.", DiagnosticId = "SYSLIB0052", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	protected internal Match? Scan(Regex regex, string text, int textbeg, int textend, int textstart, int prevlen, bool quick, TimeSpan timeout)
	{
		throw null;
	}

	protected internal virtual void Scan(ReadOnlySpan<char> text)
	{
		throw null;
	}

	protected void TransferCapture(int capnum, int uncapnum, int start, int end)
	{
	}

	protected void Uncapture()
	{
	}
}
