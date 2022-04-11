namespace System.Diagnostics
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class DebuggerTypeProxyAttribute : Attribute
	{
		public DebuggerTypeProxyAttribute(Type type)
		{

		}
	}
}
