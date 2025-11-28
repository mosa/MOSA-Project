using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Runtime.Serialization.DataContracts;

public abstract class DataContract
{
	internal const DynamicallyAccessedMemberTypes DataContractPreserveMemberTypes = DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties;

	public virtual DataContract? BaseContract
	{
		[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
		[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
		get
		{
			throw null;
		}
	}

	public virtual string? ContractType
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsBuiltInDataContract
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsISerializable
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsReference
	{
		get
		{
			throw null;
		}
	}

	public virtual bool IsValueType
	{
		get
		{
			throw null;
		}
	}

	public virtual Dictionary<XmlQualifiedName, DataContract>? KnownDataContracts
	{
		[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
		[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
		get
		{
			throw null;
		}
	}

	public virtual ReadOnlyCollection<DataMember> DataMembers
	{
		get
		{
			throw null;
		}
	}

	public virtual Type OriginalUnderlyingType
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlQualifiedName XmlName
	{
		get
		{
			throw null;
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
	public virtual Type UnderlyingType
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlDictionaryString? TopLevelElementName
	{
		get
		{
			throw null;
		}
	}

	public virtual XmlDictionaryString? TopLevelElementNamespace
	{
		get
		{
			throw null;
		}
	}

	internal DataContract(DataContractCriticalHelper helper)
	{
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public static DataContract? GetBuiltInDataContract(string name, string ns)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public static XmlQualifiedName GetXmlName(Type type)
	{
		throw null;
	}

	[RequiresDynamicCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed.")]
	[RequiresUnreferencedCode("Data Contract Serialization and Deserialization might require types that cannot be statically analyzed. Make sure all of the required types are preserved.")]
	public virtual XmlQualifiedName GetArrayTypeName(bool isNullable)
	{
		throw null;
	}

	public virtual bool IsDictionaryLike([NotNullWhen(true)] out string? keyName, [NotNullWhen(true)] out string? valueName, [NotNullWhen(true)] out string? itemName)
	{
		throw null;
	}
}
