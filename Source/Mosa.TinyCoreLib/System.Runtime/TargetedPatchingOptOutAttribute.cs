namespace System.Runtime;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class TargetedPatchingOptOutAttribute : Attribute
{
	public string Reason
	{
		get
		{
			throw null;
		}
	}

	public TargetedPatchingOptOutAttribute(string reason)
	{
	}
}
