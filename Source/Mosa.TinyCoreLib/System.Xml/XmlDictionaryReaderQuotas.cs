using System.ComponentModel;

namespace System.Xml;

public sealed class XmlDictionaryReaderQuotas
{
	public static XmlDictionaryReaderQuotas Max
	{
		get
		{
			throw null;
		}
	}

	[DefaultValue(16384)]
	public int MaxArrayLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(4096)]
	public int MaxBytesPerRead
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(32)]
	public int MaxDepth
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(16384)]
	public int MaxNameTableCharCount
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DefaultValue(8192)]
	public int MaxStringContentLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlDictionaryReaderQuotaTypes ModifiedQuotas
	{
		get
		{
			throw null;
		}
	}

	public void CopyTo(XmlDictionaryReaderQuotas quotas)
	{
	}
}
