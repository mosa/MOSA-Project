namespace System.Security;

[AttributeUsage(AttributeTargets.Module, AllowMultiple = true, Inherited = false)]
public sealed class UnverifiableCodeAttribute : Attribute
{
}
