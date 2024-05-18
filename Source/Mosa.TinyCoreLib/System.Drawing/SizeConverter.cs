using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.Drawing;

public class SizeConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
	{
		throw null;
	}

	public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
	{
		throw null;
	}

	public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
	{
		throw null;
	}

	public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
	{
		throw null;
	}

	public override object CreateInstance(ITypeDescriptorContext? context, IDictionary propertyValues)
	{
		throw null;
	}

	public override bool GetCreateInstanceSupported(ITypeDescriptorContext? context)
	{
		throw null;
	}

	[RequiresUnreferencedCode("The Type of value cannot be statically discovered. The public parameterless constructor or the 'Default' static field may be trimmed from the Attribute's Type.")]
	public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext? context, object value, Attribute[]? attributes)
	{
		throw null;
	}

	public override bool GetPropertiesSupported(ITypeDescriptorContext? context)
	{
		throw null;
	}
}
