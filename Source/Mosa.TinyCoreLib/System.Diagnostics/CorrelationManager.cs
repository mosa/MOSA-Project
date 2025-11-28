using System.Collections;

namespace System.Diagnostics;

public class CorrelationManager
{
	public Guid ActivityId
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Stack LogicalOperationStack
	{
		get
		{
			throw null;
		}
	}

	internal CorrelationManager()
	{
	}

	public void StartLogicalOperation()
	{
	}

	public void StartLogicalOperation(object operationId)
	{
	}

	public void StopLogicalOperation()
	{
	}
}
