using System.Globalization;

namespace System.ComponentModel;

public class ReferenceConverter : TypeConverter
{
	public ReferenceConverter(Type type)
	{
	}

	public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
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

	public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context)
	{
		throw null;
	}

	public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context)
	{
		throw null;
	}

	public override bool GetStandardValuesSupported(ITypeDescriptorContext? context)
	{
		throw null;
	}

	protected virtual bool IsValueAllowed(ITypeDescriptorContext context, object value)
	{
		throw null;
	}
}
