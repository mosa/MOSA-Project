using System.ComponentModel;

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class RequiresLocationAttribute : Attribute
{
}
