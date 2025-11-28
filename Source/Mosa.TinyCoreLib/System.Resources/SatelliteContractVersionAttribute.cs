using System.ComponentModel;

namespace System.Resources;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class SatelliteContractVersionAttribute : Attribute
{
	public string Version
	{
		get
		{
			throw null;
		}
	}

	public SatelliteContractVersionAttribute(string version)
	{
	}
}
