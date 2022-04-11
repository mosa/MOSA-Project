namespace System.Diagnostics.CodeAnalysis
{
	public sealed class ExcludeFromCodeCoverageAttribute : Attribute
	{
		public string Justification { get; set; }

		public ExcludeFromCodeCoverageAttribute()
		{

		}
	}
}
