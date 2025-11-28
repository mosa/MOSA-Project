namespace System.ComponentModel.Design;

public class DesignerEventArgs : EventArgs
{
	public IDesignerHost? Designer
	{
		get
		{
			throw null;
		}
	}

	public DesignerEventArgs(IDesignerHost? host)
	{
	}
}
