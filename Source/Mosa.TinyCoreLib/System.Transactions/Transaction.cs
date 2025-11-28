using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Transactions;

public class Transaction : IDisposable, ISerializable
{
	public static Transaction? Current
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IsolationLevel IsolationLevel
	{
		get
		{
			throw null;
		}
	}

	public Guid PromoterType
	{
		get
		{
			throw null;
		}
	}

	public TransactionInformation TransactionInformation
	{
		get
		{
			throw null;
		}
	}

	public event TransactionCompletedEventHandler? TransactionCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	internal Transaction()
	{
	}

	public Transaction Clone()
	{
		throw null;
	}

	public DependentTransaction DependentClone(DependentCloneOption cloneOption)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	public Enlistment EnlistDurable(Guid resourceManagerIdentifier, IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions)
	{
		throw null;
	}

	public Enlistment EnlistDurable(Guid resourceManagerIdentifier, ISinglePhaseNotification singlePhaseNotification, EnlistmentOptions enlistmentOptions)
	{
		throw null;
	}

	public bool EnlistPromotableSinglePhase(IPromotableSinglePhaseNotification promotableSinglePhaseNotification)
	{
		throw null;
	}

	public bool EnlistPromotableSinglePhase(IPromotableSinglePhaseNotification promotableSinglePhaseNotification, Guid promoterType)
	{
		throw null;
	}

	public Enlistment EnlistVolatile(IEnlistmentNotification enlistmentNotification, EnlistmentOptions enlistmentOptions)
	{
		throw null;
	}

	public Enlistment EnlistVolatile(ISinglePhaseNotification singlePhaseNotification, EnlistmentOptions enlistmentOptions)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public byte[] GetPromotedToken()
	{
		throw null;
	}

	public static bool operator ==(Transaction? x, Transaction? y)
	{
		throw null;
	}

	public static bool operator !=(Transaction? x, Transaction? y)
	{
		throw null;
	}

	public Enlistment PromoteAndEnlistDurable(Guid resourceManagerIdentifier, IPromotableSinglePhaseNotification promotableNotification, ISinglePhaseNotification enlistmentNotification, EnlistmentOptions enlistmentOptions)
	{
		throw null;
	}

	public void Rollback()
	{
	}

	public void Rollback(Exception? e)
	{
	}

	public void SetDistributedTransactionIdentifier(IPromotableSinglePhaseNotification promotableNotification, Guid distributedTransactionIdentifier)
	{
	}

	void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext context)
	{
	}
}
