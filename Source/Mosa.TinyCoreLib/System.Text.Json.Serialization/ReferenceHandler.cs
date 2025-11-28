namespace System.Text.Json.Serialization;

public abstract class ReferenceHandler
{
	public static ReferenceHandler IgnoreCycles
	{
		get
		{
			throw null;
		}
	}

	public static ReferenceHandler Preserve
	{
		get
		{
			throw null;
		}
	}

	public abstract ReferenceResolver CreateResolver();
}
public sealed class ReferenceHandler<T> : ReferenceHandler where T : ReferenceResolver, new()
{
	public override ReferenceResolver CreateResolver()
	{
		throw null;
	}
}
