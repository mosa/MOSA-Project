namespace System.Diagnostics;

public class SourceSwitch : Switch
{
	public SourceLevels Level
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SourceSwitch(string name)
		: base(null, null)
	{
	}

	public SourceSwitch(string displayName, string defaultSwitchValue)
		: base(null, null)
	{
	}

	protected override void OnValueChanged()
	{
	}

	public bool ShouldTrace(TraceEventType eventType)
	{
		throw null;
	}
}
