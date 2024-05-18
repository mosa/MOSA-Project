using System.Collections;

namespace System.ComponentModel.Design;

public class MenuCommand
{
	public virtual bool Checked
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual CommandID? CommandID
	{
		get
		{
			throw null;
		}
	}

	public virtual bool Enabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual int OleStatus
	{
		get
		{
			throw null;
		}
	}

	public virtual IDictionary Properties
	{
		get
		{
			throw null;
		}
	}

	public virtual bool Supported
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool Visible
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event EventHandler? CommandChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public MenuCommand(EventHandler? handler, CommandID? command)
	{
	}

	public virtual void Invoke()
	{
	}

	public virtual void Invoke(object arg)
	{
	}

	protected virtual void OnCommandChanged(EventArgs e)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
