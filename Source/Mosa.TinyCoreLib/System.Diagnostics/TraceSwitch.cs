namespace System.Diagnostics;

[SwitchLevel(typeof(TraceLevel))]
public class TraceSwitch : Switch
{
	public TraceLevel Level
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool TraceError
	{
		get
		{
			throw null;
		}
	}

	public bool TraceInfo
	{
		get
		{
			throw null;
		}
	}

	public bool TraceVerbose
	{
		get
		{
			throw null;
		}
	}

	public bool TraceWarning
	{
		get
		{
			throw null;
		}
	}

	public TraceSwitch(string displayName, string? description)
		: base(null, null)
	{
	}

	public TraceSwitch(string displayName, string? description, string defaultSwitchValue)
		: base(null, null)
	{
	}

	protected override void OnSwitchSettingChanged()
	{
	}

	protected override void OnValueChanged()
	{
	}
}
