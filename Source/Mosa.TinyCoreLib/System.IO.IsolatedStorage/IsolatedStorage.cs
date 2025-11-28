namespace System.IO.IsolatedStorage;

public abstract class IsolatedStorage : MarshalByRefObject
{
	public object ApplicationIdentity
	{
		get
		{
			throw null;
		}
	}

	public object AssemblyIdentity
	{
		get
		{
			throw null;
		}
	}

	public virtual long AvailableFreeSpace
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	[Obsolete("IsolatedStorage.CurrentSize has been deprecated because it is not CLS Compliant. To get the current size use IsolatedStorage.UsedSize instead.")]
	public virtual ulong CurrentSize
	{
		get
		{
			throw null;
		}
	}

	public object DomainIdentity
	{
		get
		{
			throw null;
		}
	}

	[CLSCompliant(false)]
	[Obsolete("IsolatedStorage.MaximumSize has been deprecated because it is not CLS Compliant. To get the maximum size use IsolatedStorage.Quota instead.")]
	public virtual ulong MaximumSize
	{
		get
		{
			throw null;
		}
	}

	public virtual long Quota
	{
		get
		{
			throw null;
		}
	}

	public IsolatedStorageScope Scope
	{
		get
		{
			throw null;
		}
	}

	protected virtual char SeparatorExternal
	{
		get
		{
			throw null;
		}
	}

	protected virtual char SeparatorInternal
	{
		get
		{
			throw null;
		}
	}

	public virtual long UsedSize
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IncreaseQuotaTo(long newQuotaSize)
	{
		throw null;
	}

	protected void InitStore(IsolatedStorageScope scope, Type appEvidenceType)
	{
	}

	protected void InitStore(IsolatedStorageScope scope, Type? domainEvidenceType, Type? assemblyEvidenceType)
	{
	}

	public abstract void Remove();
}
