using System;
using System.ComponentModel;

namespace Microsoft.CSharp.RuntimeBinder;

[EditorBrowsable(EditorBrowsableState.Never)]
[Flags]
public enum CSharpArgumentInfoFlags
{
	None = 0,
	UseCompileTimeType = 1,
	Constant = 2,
	NamedArgument = 4,
	IsRef = 8,
	IsOut = 0x10,
	IsStaticType = 0x20
}
