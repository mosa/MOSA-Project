namespace System.Reflection;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyCopyrightAttribute : Attribute
{
	public string Copyright
	{
		get
		{
			throw null;
		}
	}

	public AssemblyCopyrightAttribute(string copyright)
	{
	}
}
