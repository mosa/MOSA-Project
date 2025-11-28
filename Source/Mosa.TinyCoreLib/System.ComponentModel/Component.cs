namespace System.ComponentModel;

[DesignerCategory("Component")]
public class Component : MarshalByRefObject, IComponent, IDisposable
{
	protected virtual bool CanRaiseEvents
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IContainer? Container
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	protected bool DesignMode
	{
		get
		{
			throw null;
		}
	}

	protected EventHandlerList Events
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual ISite? Site
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public event EventHandler? Disposed
	{
		add
		{
		}
		remove
		{
		}
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~Component()
	{
	}

	protected virtual object? GetService(Type service)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
