using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System.ComponentModel;

public abstract class TypeListConverter : TypeConverter
{
	protected TypeListConverter(Type[] types)
	{
	}

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
}
