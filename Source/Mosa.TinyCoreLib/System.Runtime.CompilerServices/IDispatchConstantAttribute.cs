using System.Runtime.Versioning;

namespace System.Runtime.CompilerServices;

[SupportedOSPlatform("windows")]
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
public sealed class IDispatchConstantAttribute : CustomConstantAttribute
{
	public override object Value
	{
		get
		{
			throw null;
		}
	}
}
