using System.Runtime.InteropServices;

namespace System.Speech.Synthesis.TtsEngine;

[StructLayout(LayoutKind.Sequential)]
public class Prosody
{
	public int Duration
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ProsodyNumber Pitch
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ProsodyNumber Range
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ProsodyNumber Rate
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ProsodyNumber Volume
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ContourPoint[] GetContourPoints()
	{
		throw null;
	}

	public void SetContourPoints(ContourPoint[] points)
	{
	}
}
