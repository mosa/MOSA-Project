namespace System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class EditableAttribute : Attribute
{
	public bool AllowEdit
	{
		get
		{
			throw null;
		}
	}

	public bool AllowInitialValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EditableAttribute(bool allowEdit)
	{
	}
}
