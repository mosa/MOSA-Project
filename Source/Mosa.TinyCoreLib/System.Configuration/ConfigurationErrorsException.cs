using System.Collections;
using System.Runtime.Serialization;
using System.Xml;

namespace System.Configuration;

public class ConfigurationErrorsException : ConfigurationException
{
	public override string BareMessage
	{
		get
		{
			throw null;
		}
	}

	public ICollection Errors
	{
		get
		{
			throw null;
		}
	}

	public override string Filename
	{
		get
		{
			throw null;
		}
	}

	public override int Line
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

	public ConfigurationErrorsException()
	{
	}

	protected ConfigurationErrorsException(SerializationInfo info, StreamingContext context)
	{
	}

	public ConfigurationErrorsException(string message)
	{
	}

	public ConfigurationErrorsException(string message, Exception inner)
	{
	}

	public ConfigurationErrorsException(string message, Exception inner, string filename, int line)
	{
	}

	public ConfigurationErrorsException(string message, Exception inner, XmlNode node)
	{
	}

	public ConfigurationErrorsException(string message, Exception inner, XmlReader reader)
	{
	}

	public ConfigurationErrorsException(string message, string filename, int line)
	{
	}

	public ConfigurationErrorsException(string message, XmlNode node)
	{
	}

	public ConfigurationErrorsException(string message, XmlReader reader)
	{
	}

	public static string GetFilename(XmlNode node)
	{
		throw null;
	}

	public static string GetFilename(XmlReader reader)
	{
		throw null;
	}

	public static int GetLineNumber(XmlNode node)
	{
		throw null;
	}

	public static int GetLineNumber(XmlReader reader)
	{
		throw null;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
