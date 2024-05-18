using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class OptionCompareAttribute : Attribute
{
}
