namespace System.ComponentModel;

[Designer("System.Windows.Forms.Design.ComponentDocumentDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.IRootDesigner, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
[DesignerCategory("Component")]
[TypeConverter(typeof(ComponentConverter))]
public class MarshalByValueComponent : IComponent, IDisposable, IServiceProvider
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual IContainer? Container
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool DesignMode
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

	~MarshalByValueComponent()
	{
	}

	public virtual object? GetService(Type service)
	{
		throw null;
	}

	public override string? ToString()
	{
		throw null;
	}
}
