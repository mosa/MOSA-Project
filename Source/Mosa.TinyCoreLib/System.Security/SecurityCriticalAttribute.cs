namespace System.Security;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
public sealed class SecurityCriticalAttribute : Attribute
{
	[Obsolete("SecurityCriticalScope is only used for .NET 2.0 transparency compatibility.")]
	public SecurityCriticalScope Scope { get; }

	public SecurityCriticalAttribute() { }

	public SecurityCriticalAttribute(SecurityCriticalScope scope) => Scope = scope;
}
