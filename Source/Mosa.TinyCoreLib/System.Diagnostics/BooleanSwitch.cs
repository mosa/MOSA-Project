namespace System.Diagnostics;

[SwitchLevel(typeof(bool))]
public class BooleanSwitch : Switch
{
	public bool Enabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public BooleanSwitch(string displayName, string? description)
		: base(null, null)
	{
	}

	public BooleanSwitch(string displayName, string? description, string defaultSwitchValue)
		: base(null, null)
	{
	}

	protected override void OnValueChanged()
	{
	}
}
