namespace System.Management;

public class PutOptions : ManagementOptions
{
	public PutType Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool UseAmendedQualifiers
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PutOptions()
	{
	}

	public PutOptions(ManagementNamedValueCollection context)
	{
	}

	public PutOptions(ManagementNamedValueCollection context, TimeSpan timeout, bool useAmendedQualifiers, PutType putType)
	{
	}

	public override object Clone()
	{
		throw null;
	}
}
