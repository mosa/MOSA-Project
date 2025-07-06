using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Reflection;

public class ParameterInfo : ICustomAttributeProvider, IObjectReference
{
	protected ParameterAttributes AttrsImpl;

	protected Type? ClassImpl;

	protected object? DefaultValueImpl;

	protected MemberInfo MemberImpl;

	protected string? NameImpl;

	protected int PositionImpl;

	public virtual ParameterAttributes Attributes
	{
		get
		{
			throw null;
		}
	}

	public virtual IEnumerable<CustomAttributeData> CustomAttributes
	{
		get
		{
			throw null;
		}
	}

	public virtual object? DefaultValue
	{
		get
		{
			throw null;
		}
	}

	public virtual bool HasDefaultValue => (Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault;

	public bool IsIn => (Attributes & ParameterAttributes.In) == ParameterAttributes.In;

	public bool IsLcid => (Attributes & ParameterAttributes.Lcid) == ParameterAttributes.Lcid;

	public bool IsOptional => (Attributes & ParameterAttributes.Optional) == ParameterAttributes.Optional;

	public bool IsOut => (Attributes & ParameterAttributes.Out) == ParameterAttributes.Out;

	public bool IsRetval => (Attributes & ParameterAttributes.Retval) == ParameterAttributes.Retval;

	public virtual MemberInfo Member
	{
		get
		{
			throw null;
		}
	}

	public virtual int MetadataToken
	{
		get
		{
			throw null;
		}
	}

	public virtual string? Name
	{
		get
		{
			throw null;
		}
	}

	public virtual Type ParameterType
	{
		get
		{
			throw null;
		}
	}

	public virtual int Position
	{
		get
		{
			throw null;
		}
	}

	public virtual object? RawDefaultValue
	{
		get
		{
			throw null;
		}
	}

	protected ParameterInfo()
	{
	}

	public virtual object[] GetCustomAttributes(bool inherit)
	{
		throw null;
	}

	public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw null;
	}

	public virtual IList<CustomAttributeData> GetCustomAttributesData()
	{
		throw null;
	}

	public virtual Type GetModifiedParameterType()
	{
		throw null;
	}

	public virtual Type[] GetOptionalCustomModifiers()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public object GetRealObject(StreamingContext context)
	{
		throw null;
	}

	public virtual Type[] GetRequiredCustomModifiers()
	{
		throw null;
	}

	public virtual bool IsDefined(Type attributeType, bool inherit)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
