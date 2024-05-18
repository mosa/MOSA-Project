namespace System.ComponentModel.Design.Serialization;

public abstract class DesignerLoader
{
	public virtual bool Loading
	{
		get
		{
			throw null;
		}
	}

	public abstract void BeginLoad(IDesignerLoaderHost host);

	public abstract void Dispose();

	public virtual void Flush()
	{
	}
}
