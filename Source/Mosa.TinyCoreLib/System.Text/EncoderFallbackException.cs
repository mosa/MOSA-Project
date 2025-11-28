namespace System.Text;

public sealed class EncoderFallbackException : ArgumentException
{
	public char CharUnknown
	{
		get
		{
			throw null;
		}
	}

	public char CharUnknownHigh
	{
		get
		{
			throw null;
		}
	}

	public char CharUnknownLow
	{
		get
		{
			throw null;
		}
	}

	public int Index
	{
		get
		{
			throw null;
		}
	}

	public EncoderFallbackException()
	{
	}

	public EncoderFallbackException(string? message)
	{
	}

	public EncoderFallbackException(string? message, Exception? innerException)
	{
	}

	public bool IsUnknownSurrogate()
	{
		throw null;
	}
}
