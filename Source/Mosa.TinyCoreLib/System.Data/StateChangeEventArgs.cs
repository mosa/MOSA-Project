namespace System.Data;

public sealed class StateChangeEventArgs : EventArgs
{
	public ConnectionState CurrentState
	{
		get
		{
			throw null;
		}
	}

	public ConnectionState OriginalState
	{
		get
		{
			throw null;
		}
	}

	public StateChangeEventArgs(ConnectionState originalState, ConnectionState currentState)
	{
	}
}
