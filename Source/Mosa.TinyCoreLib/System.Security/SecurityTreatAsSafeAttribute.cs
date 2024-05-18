namespace System.Security;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
[Obsolete("SecurityTreatAsSafe is only used for .NET 2.0 transparency compatibility. Use the SecuritySafeCriticalAttribute instead.")]
public sealed class SecurityTreatAsSafeAttribute : Attribute
{
}
