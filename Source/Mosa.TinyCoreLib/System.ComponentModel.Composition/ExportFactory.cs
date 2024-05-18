namespace System.ComponentModel.Composition;

public class ExportFactory<T>
{
	public ExportFactory(Func<Tuple<T, Action>> exportLifetimeContextCreator)
	{
	}

	public ExportLifetimeContext<T> CreateExport()
	{
		throw null;
	}
}
public class ExportFactory<T, TMetadata> : ExportFactory<T>
{
	public TMetadata Metadata
	{
		get
		{
			throw null;
		}
	}

	public ExportFactory(Func<Tuple<T, Action>> exportLifetimeContextCreator, TMetadata metadata)
		: base((Func<Tuple<T, Action>>)null)
	{
	}
}
