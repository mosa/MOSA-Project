namespace System.ComponentModel.Design;

public class ActiveDesignerEventArgs : EventArgs
{
	public IDesignerHost? NewDesigner
	{
		get
		{
			throw null;
		}
	}

	public IDesignerHost? OldDesigner
	{
		get
		{
			throw null;
		}
	}

	public ActiveDesignerEventArgs(IDesignerHost? oldDesigner, IDesignerHost? newDesigner)
	{
	}
}
