using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Linq;

[TypeDescriptionProvider("MS.Internal.Xml.Linq.ComponentModel.XTypeDescriptionProvider`1[[System.Xml.Linq.XAttribute, System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]],System.ComponentModel.TypeConverter")]
public class XAttribute : XObject
{
	public static IEnumerable<XAttribute> EmptySequence
	{
		get
		{
			throw null;
		}
	}

	public bool IsNamespaceDeclaration
	{
		get
		{
			throw null;
		}
	}

	public XName Name
	{
		get
		{
			throw null;
		}
	}

	public XAttribute? NextAttribute
	{
		get
		{
			throw null;
		}
	}

	public override XmlNodeType NodeType
	{
		get
		{
			throw null;
		}
	}

	public XAttribute? PreviousAttribute
	{
		get
		{
			throw null;
		}
	}

	public string Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XAttribute(XAttribute other)
	{
	}

	public XAttribute(XName name, object value)
	{
	}

	[CLSCompliant(false)]
	public static explicit operator bool(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator DateTime(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator DateTimeOffset(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator decimal(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator double(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Guid(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator int(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator long(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator bool?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator DateTimeOffset?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator DateTime?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator decimal?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator double?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator Guid?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator int?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator long?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator float?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator TimeSpan?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator uint?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator ulong?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator float(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("attribute")]
	public static explicit operator string?(XAttribute? attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator TimeSpan(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator uint(XAttribute attribute)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator ulong(XAttribute attribute)
	{
		throw null;
	}

	public void Remove()
	{
	}

	public void SetValue(object value)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
