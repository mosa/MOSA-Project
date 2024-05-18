using System.Runtime.Serialization;
using System.Xml;

namespace System.Configuration;

public class ConfigurationException : SystemException
{
	public virtual string BareMessage
	{
		get
		{
			throw null;
		}
	}

	public virtual string Filename
	{
		get
		{
			throw null;
		}
	}

	public virtual int Line
	{
		get
		{
			throw null;
		}
	}

	public override string Message
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
	public ConfigurationException()
	{
	}

	protected ConfigurationException(SerializationInfo info, StreamingContext context)
	{
	}

	[Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
	public ConfigurationException(string message)
	{
	}

	[Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
	public ConfigurationException(string message, Exception inner)
	{
	}

	[Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
	public ConfigurationException(string message, Exception inner, string filename, int line)
	{
	}

	[Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
	public ConfigurationException(string message, Exception inner, XmlNode node)
	{
	}

	[Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
	public ConfigurationException(string message, string filename, int line)
	{
	}

	[Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException instead.")]
	public ConfigurationException(string message, XmlNode node)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	[Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException.GetFilename instead.")]
	public static string GetXmlNodeFilename(XmlNode node)
	{
		throw null;
	}

	[Obsolete("ConfigurationException has been deprecated. Use System.Configuration.ConfigurationErrorsException.GetLinenumber instead.")]
	public static int GetXmlNodeLineNumber(XmlNode node)
	{
		throw null;
	}
}
