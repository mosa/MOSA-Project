using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class OptionTextAttribute : Attribute
{
}
