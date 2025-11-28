namespace System.Reflection.Emit;

public enum FlowControl
{
	Branch,
	Break,
	Call,
	Cond_Branch,
	Meta,
	Next,
	[Obsolete("FlowControl.Phi has been deprecated and is not supported.")]
	Phi,
	Return,
	Throw
}
