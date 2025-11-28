namespace System.Runtime;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyTargetedPatchBandAttribute : Attribute
{
	public string TargetedPatchBand
	{
		get
		{
			throw null;
		}
	}

	public AssemblyTargetedPatchBandAttribute(string targetedPatchBand)
	{
	}
}
