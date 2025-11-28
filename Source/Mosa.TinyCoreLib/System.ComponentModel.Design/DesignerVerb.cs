namespace System.ComponentModel.Design;

public class DesignerVerb : MenuCommand
{
	public string Description
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Text
	{
		get
		{
			throw null;
		}
	}

	public DesignerVerb(string text, EventHandler handler)
		: base(null, null)
	{
	}

	public DesignerVerb(string text, EventHandler handler, CommandID startCommandID)
		: base(null, null)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
