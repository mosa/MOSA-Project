using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Diagnostics;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public sealed class EventLogEntry : Component, ISerializable
{
	public string Category
	{
		get
		{
			throw null;
		}
	}

	public short CategoryNumber
	{
		get
		{
			throw null;
		}
	}

	public byte[] Data
	{
		get
		{
			throw null;
		}
	}

	public EventLogEntryType EntryType
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("EventLogEntry.EventID has been deprecated. Use System.Diagnostics.EventLogEntry.InstanceId instead.")]
	public int EventID
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

	public long InstanceId
	{
		get
		{
			throw null;
		}
	}

	public string MachineName
	{
		get
		{
			throw null;
		}
	}

	[Editor("System.ComponentModel.Design.BinaryEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public string Message
	{
		get
		{
			throw null;
		}
	}

	public string[] ReplacementStrings
	{
		get
		{
			throw null;
		}
	}

	public string Source
	{
		get
		{
			throw null;
		}
	}

	public DateTime TimeGenerated
	{
		get
		{
			throw null;
		}
	}

	public DateTime TimeWritten
	{
		get
		{
			throw null;
		}
	}

	public string UserName
	{
		get
		{
			throw null;
		}
	}

	internal EventLogEntry()
	{
	}

	public bool Equals(EventLogEntry otherEntry)
	{
		throw null;
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
