namespace System.Diagnostics
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public sealed class DebuggerDisplayAttribute : Attribute
	{
		public DebuggerDisplayAttribute(string text)
		{
			
		}
	}
}
