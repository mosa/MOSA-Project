using System.ComponentModel;

namespace System.Text.RegularExpressions;

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class RegexRunnerFactory
{
	protected internal abstract RegexRunner CreateInstance();
}
