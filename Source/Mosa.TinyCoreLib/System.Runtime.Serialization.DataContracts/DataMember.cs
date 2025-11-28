using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Serialization.DataContracts;

public sealed class DataMember
{
	public bool EmitDefaultValue
	{
		get
		{
			throw null;
		}
	}

	public bool IsNullable
	{
		get
		{
			throw null;
		}
	}

	public bool IsRequired
	{
		get
		{
			throw null;
		}
	}

	public DataContract MemberTypeContract
	{
		[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
		[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public long Order
	{
		get
		{
			throw null;
		}
	}

	internal DataMember()
	{
	}
}
