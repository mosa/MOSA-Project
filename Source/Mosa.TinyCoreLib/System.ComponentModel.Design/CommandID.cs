using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel.Design;

public class CommandID
{
	public virtual Guid Guid
	{
		get
		{
			throw null;
		}
	}

	public virtual int ID
	{
		get
		{
			throw null;
		}
	}

	public CommandID(Guid menuGroup, int commandID)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
