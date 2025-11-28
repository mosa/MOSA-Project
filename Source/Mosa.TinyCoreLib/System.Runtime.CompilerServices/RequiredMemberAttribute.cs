using System.ComponentModel;

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class RequiredMemberAttribute : Attribute
{
}
